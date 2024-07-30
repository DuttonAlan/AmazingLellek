using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;
using PlanB.DPF.Manager.Command.Host;
using System.Net;

namespace PlanB.DPF.Manager.Function.Functions;

public class ClockFunction
{
    private readonly ClockCommandHost clockHost; 


    public ClockFunction(ClockCommandHost clockHost)
    {
        this.clockHost = clockHost;
    }


    [Function("GetDate")]
    [OpenApiOperation(operationId: "GetDate", tags: new[] { "date" }, Visibility = OpenApiVisibilityType.Important, Description = "Getting current date.")]
    [OpenApiSecurity(schemeName: "bearer_auth", schemeType: SecuritySchemeType.Http, BearerFormat = "JWT", Scheme = OpenApiSecuritySchemeType.Bearer)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DateDTO))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "Internal error occurs.")]
    public async Task<HttpResponseData> GetDate([HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "clocks/date")] HttpRequestData req, FunctionContext executionContext)
    {
        DateDTO content = await this.clockHost.GetDate();

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(content);
        return response;
    }


    [Function("GetTime")]
    [OpenApiOperation(operationId: "GetTime", tags: new[] { "time" }, Visibility = OpenApiVisibilityType.Important, Description = "Getting current time.")]
    [OpenApiSecurity(schemeName: "bearer_auth", schemeType: SecuritySchemeType.Http, BearerFormat = "JWT", Scheme = OpenApiSecuritySchemeType.Bearer)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TimeDTO))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "Internal error occurs.")]
    public async Task<HttpResponseData> GetTime([HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "clocks/time")] HttpRequestData req, FunctionContext executionContext)
    {
        TimeDTO content = await this.clockHost.GetTime();

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK); 
        await response.WriteAsJsonAsync(content);
        return response;
    }
}
