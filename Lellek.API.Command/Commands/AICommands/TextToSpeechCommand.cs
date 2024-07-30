using PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;
using PlanB.DPF.Manager.Core;
using PlanB.PDF.Manager.Service.Interfaces;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace PlanB.DPF.Manager.Command.Commands.AICommands;

public class TextToSpeechCommand : Command<string, HttpResponseMessage>
{
    private readonly IHttpRequestService requestService;
    private readonly string openAiApiKey;
    private readonly string openAiUrl;

    public TextToSpeechCommand(IHttpRequestService requestService)
    {
        this.requestService = requestService;
        this.openAiApiKey = Consts.Oai_API_KEY;
        this.openAiUrl = Consts.Oai_speech_API_URL;
    }

    public async override Task<HttpResponseMessage> Execute(string answer)
    {
        var requestData = new { model = "tts-1", voice = "onyx", input = answer};
        var requestContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

        requestService.SetHeader("Authorization", $"Bearer {openAiApiKey}");

        var response = await requestService.GetAudio(HttpMethod.Post, openAiUrl, requestContent);
        return response;
    }
}
