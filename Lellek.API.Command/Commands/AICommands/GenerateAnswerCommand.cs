using Newtonsoft.Json;
using PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;
using PlanB.DPF.Manager.Command.Host;
using PlanB.DPF.Manager.Core;
using PlanB.PDF.Manager.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlanB.DPF.Manager.Command.Commands.AICommands;

public class GenerateAnswerCommand : Command<string, string>
{
    private readonly IHttpRequestService requestService;
    private readonly WeatherCommandHost weatherHost;
    private readonly string openAiApiKey;
    private readonly string openAiUrl;
    private readonly List<object> messageHistory;

    private Task<TemperatureDTO> temperature;

    public GenerateAnswerCommand(IHttpRequestService requestService, WeatherCommandHost weatherHost)
    {
        this.requestService = requestService;
        this.weatherHost = weatherHost;
        this.openAiApiKey = Consts.Oai_API_KEY; 
        this.openAiUrl = Consts.Oai_chat_API_URL;
        this.temperature = this.weatherHost.GetTemperature();
        this.messageHistory = new List<object> 
        { 
            new 
            { 
                role = "system", 
                content = Consts.Oai_Clock_Prompt + $" Information: Datum =  {DateOnly.FromDateTime(DateTime.Now)}, " +
                $"Uhrzeit = {TimeOnly.FromDateTime(DateTime.Now)}, " +
                $"Temperatur = Current:{this.temperature.Result.Value},Min:{this.temperature.Result.Min},Max:{this.temperature.Result.Max}" +
                $"Wetter = {this.temperature.Result.Category}"
            } 
        };
    }

    public async override Task<string> Execute(string prompt)
    {
        if (messageHistory.Count > 11)
        {
            messageHistory.RemoveRange(1, 2);
        }

        messageHistory.Add(new {role = "user", content =  prompt});
        var requestData = new
        {
            messages = messageHistory,
            max_tokens = 2000,
            model = "gpt-3.5-turbo",
            temperature = 0.5
        };
        var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

        requestService.SetHeader("Authorization", $"Bearer {openAiApiKey}");

        var response = await requestService.GetContent(HttpMethod.Post, openAiUrl, requestContent);

        if (response is Exception || response == null)
        {
            return response;
        }
        else
        {
            var content = response.choices[0].message.content;
            messageHistory.Add(new { role = "assistant", content = content });
            return content;
        }

    }
}

