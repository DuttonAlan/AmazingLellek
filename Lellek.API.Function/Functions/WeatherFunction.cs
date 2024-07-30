using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using PlanB.DPF.Manager.Command.Host;
using System.Net;
using PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;

namespace PlanB.DPF.Manager.Function.Functions;

public class WeatherFunction
{
    private readonly WeatherCommandHost weatherHost;


    public WeatherFunction(WeatherCommandHost weatherHost) 
    {
        this.weatherHost = weatherHost;
    }


    [Function("GetTemperature")]
    [OpenApiOperation(operationId: "GetTemperature", tags: new[] { "temperature" }, Visibility = OpenApiVisibilityType.Important, Description = "Getting current temperatures.")]
    [OpenApiSecurity(schemeName: "bearer_auth", schemeType: SecuritySchemeType.Http, BearerFormat = "JWT", Scheme = OpenApiSecuritySchemeType.Bearer)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TemperatureDTO))] 
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "Internal error occurs.")]
    public async Task<HttpResponseData> GetWeatherTemperature([HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "weathers/temperature")] HttpRequestData req, FunctionContext executionContext)
    {
        TemperatureDTO content = await this.weatherHost.GetTemperature();

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK); 
        await response.WriteAsJsonAsync(content); 
        return response;
    }
}
