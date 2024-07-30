using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;
using Microsoft.IdentityModel.Protocols;
using HttpRequestData = Microsoft.Azure.Functions.Worker.Http.HttpRequestData;

namespace PlanB.DPF.Manager.Function.Middleware;

public class SwaggerInterceptorMiddleware: IFunctionsWorkerMiddleware
{

    private readonly ConfigurationManager<OpenIdConnectConfiguration> _configurationManager;
    private readonly bool _isSwaggerUiDisabled = false;

    public SwaggerInterceptorMiddleware(IConfiguration configuration)
    {
        _isSwaggerUiDisabled = Convert.ToBoolean(configuration["Liebherr:SwaggerUi:Disable"]);
    }
  
    
    private HttpRequestData? GetHttpRequestData(FunctionContext funcContext)

    {
        var kv = funcContext?.Features
            .SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
        var funcBinding = kv?.Value;
        var type = funcBinding?.GetType();
        var inputData = type?.GetProperties()
            .Single(p => p.Name == "InputData")
            .GetValue(funcBinding) as IReadOnlyDictionary<string, object>;
        return inputData?.Values?.SingleOrDefault() as HttpRequestData;
    }
    
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var httpRequestData = GetHttpRequestData(context);
        // url includes "swagger" and the configuration flag is true then hide the page
        if (_isSwaggerUiDisabled && (httpRequestData != null && httpRequestData.Url.AbsoluteUri.Contains("swagger")))
        {
            context.SetHttpResponseStatusCode(HttpStatusCode.Unauthorized);
            return;
        }
        // go on
        await next(context);
    }
}