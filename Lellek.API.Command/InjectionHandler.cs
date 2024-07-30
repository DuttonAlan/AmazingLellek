using Microsoft.Extensions.DependencyInjection;
using PlanB.DPF.Manager.Command.Host;

namespace PlanB.DPF.Manager.Command;

public static class InjectionHandler
{
    /// <summary>
    ///     Add the command host into the container.
    /// </summary>
    /// <param name="services"></param>
    public static void AddCommandHost(this IServiceCollection services)
    {
        // inject the command host
        services.AddTransient<WeatherCommandHost>();
        services.AddTransient<ClockCommandHost>();
        services.AddTransient<AICommandHost>();
        // register the command steps
        CommandInitialization.RegisterCommands(services);
    }
}