using PlanB.DPF.Manager.Command;
using PlanB.DPF.Manager.Function.Mapping;
using PlanB.DPF.Manager.Function.Middleware;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlanB.PDF.Manager.Service.Interfaces;
using PlanB.PDF.Manager.Service.Services;
using PlanB.DPF.Manager.Command.Host;
using PlanB.DPF.Manager.Command.Commands.AICommands;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseNewtonsoftJson();
        worker.UseMiddleware<AuthenticationMiddleware>();
        worker.UseMiddleware<SwaggerInterceptorMiddleware>();
    })
    .ConfigureServices(services =>
    {
        // Add Logging
        services.AddLogging();

        // Add HttpClient
        services.AddHttpClient();

        // add command host
        services.AddCommandHost(); 

        // add CORS
        services.AddCors(); 

        var sp = services.BuildServiceProvider();
        var configuration = sp.GetService<IConfiguration>();

        // TODO: Add database context
        // services.AddPmdDataAccess(configuration.GetConnectionString("SQLConnection"));

        services.AddTransient<IHttpRequestService, HttpRequestService>();


        // setup automapper
        services.AddAutoMapper(
            cfg => cfg.AddProfile<MappingProfile>(),
            AppDomain.CurrentDomain.GetAssemblies());

        services.AddMemoryCache();

        services.AddSingleton<AICommandHost>();
        services.AddTransient<SpeechToTextCommand>();

    })
    .ConfigureOpenApi()
    .ConfigureAppConfiguration(configurationBuilder => { configurationBuilder.AddCommandLine(args); })
    .UseDefaultServiceProvider((context, options) => { options.ValidateScopes = context.HostingEnvironment.IsDevelopment(); })
    .Build();

host.Run();