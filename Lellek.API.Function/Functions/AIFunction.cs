using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Net;
using PlanB.DPF.Manager.Command.Host;

namespace PlanB.DPF.Manager.Function.Functions;

public class SpeechToTextFunction
{
    private readonly AICommandHost _AICommandHost;

    public SpeechToTextFunction(AICommandHost aiCommandHost)
    {
        _AICommandHost = aiCommandHost;
    }

    [Function("GenerateBotAnswer")]
    [OpenApiOperation(operationId: "GenerateBotAnswer", tags: new[] { "speech-to-text" }, Visibility = OpenApiVisibilityType.Important, Description = "Converts MP3 to text and sends it to Azure OpenAI.")]
    [OpenApiSecurity(schemeName: "bearer_auth", schemeType: SecuritySchemeType.Http, BearerFormat = "JWT", Scheme = OpenApiSecuritySchemeType.Bearer)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(byte[]))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "Internal error occurs.")]
    public async Task<byte[]> GenerateBotAnswer([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "speech/convert")] HttpRequestData req, FunctionContext executionContext)
    {
        var binary = new BinaryReader(req.Body).ReadBytes((int)req.Body.Length); 

        string mp3TextContent = this._AICommandHost.ConvertMp3ToText(binary).Result;

        string botAnswer = this._AICommandHost.GenerateTextAnswer(mp3TextContent).Result;

        HttpResponseMessage speechAnswer = await this._AICommandHost.GenerateSpeechAnswer(botAnswer);

        byte[] content = await speechAnswer.Content.ReadAsByteArrayAsync();

        return content;
    }
}


