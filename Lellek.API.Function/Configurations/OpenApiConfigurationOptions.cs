using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace PlanB.DPF.Manager.Function.Configurations;

public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
{
    public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
    {
        Version = GetOpenApiDocVersion(),
        Title = GetOpenApiDocTitle(),
        Description = "API to handle the data for the PMD Manager.",
        Contact = new OpenApiContact()
        {
            Name = "LHB EMT",
            Email = "heiko.huber@plan-b-gmbh.com"
        },
        License = new OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("http://opensource.org/licenses/MIT"),
        }
    };

    public override OpenApiVersionType OpenApiVersion { get; set; } = GetOpenApiVersion();
}