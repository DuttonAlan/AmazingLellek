using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using HttpRequestData = Microsoft.Azure.Functions.Worker.Http.HttpRequestData;

namespace PlanB.DPF.Manager.Function.Middleware;

public class AuthenticationMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ConfigurationManager<OpenIdConnectConfiguration> _configurationManager;


    public AuthenticationMiddleware(IConfiguration configuration)
    {

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
        // url includes "swagger", ignore the authentication
        if (httpRequestData != null && httpRequestData.Url.AbsoluteUri.Contains("swagger"))
        {
            await next(context);
        }
        else
        {
            // if (!TryGetTokenFromHeaders(context, out var token))
            // {
            //     // Unable to get token from headers
            //     context.SetHttpResponseStatusCode(HttpStatusCode.Forbidden);
            //     return;
            // }
            
            try
            {
                // // convert to JWT token
                // var handler = new JwtSecurityTokenHandler();
                // var jsonToken = handler.ReadToken(token);
                // var jToken = jsonToken as JwtSecurityToken;
                //
                // // users upn
                // var userUpn = jToken.Subject;
                // // TODO: do TOKEN validation
                // // if (string.IsNullOrEmpty(userUpn))
                // // {
                // //     throw new InvalidDataException("Could not read the UPN from the given token.");
                // // }
                
                // go on
                await next(context);
            }
            catch (SecurityTokenException)
            {
                // Token is not valid (expired etc.)
                context.SetHttpResponseStatusCode(HttpStatusCode.Forbidden);
                return;
            }
        }
    }

    private static bool TryGetTokenFromHeaders(FunctionContext context, out string token)
    {
        token = null;
        // HTTP headers are in the binding context as a JSON object
        // The first checks ensure that we have the JSON string
        if (!context.BindingContext.BindingData.TryGetValue("Headers", out var headersObj))
        {
            return false;
        }

        if (headersObj is not string headersStr)
        {
            return false;
        }

        // Deserialize headers from JSON
        var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(headersStr);
        var normalizedKeyHeaders = headers.ToDictionary(h => h.Key.ToLowerInvariant(), h => h.Value);
        if (!normalizedKeyHeaders.TryGetValue("authorization", out var authHeaderValue))
        {
            // No Authorization header present
            return false;
        }

        if (!authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            // Scheme is not Bearer
            return false;
        }

        token = authHeaderValue.Substring("Bearer ".Length).Trim();
        return true;
    }
}