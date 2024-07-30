using Azure.Core;
using PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;
using PlanB.DPF.Manager.Core;
using PlanB.PDF.Manager.Service.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;


namespace PlanB.DPF.Manager.Command.Commands.AICommands;

public class SpeechToTextCommand : Command<byte[], string>
{
    private readonly IHttpRequestService requestService;
    private readonly string openAiApiKey;
    private readonly string openAiUrl;

    public SpeechToTextCommand(IHttpRequestService requestService)
    {
        this.requestService = requestService;
        this.openAiApiKey = Consts.Oai_API_KEY;
        this.openAiUrl = Consts.Oai_whisper_API_URL;
    }

    public async override Task<string> Execute(byte[] mp3File)
    {
        var apiKey = Consts.Oai_API_KEY;

        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.openai.com/v1/audio/transcriptions?model=whisper-1"))
            {
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {apiKey}");

                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new StringContent("whisper-1"), "model");
                multipartContent.Add(new ByteArrayContent(mp3File), "file", "audio.mp3");
                request.Content = multipartContent;
                            
                var response = await httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                return responseContent;
            }
        }
    }
}
