using Microsoft.Extensions.DependencyInjection;
using PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;
using PlanB.DPF.Manager.Command.Commands;
using PlanB.DPF.Manager.Command.Commands.WeatherCommands;
using System;
using System.Threading.Tasks;

namespace PlanB.DPF.Manager.Command.Host;

public partial class WeatherCommandHost
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="serviceProvider"></param>
    public WeatherCommandHost(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TemperatureDTO> GetTemperature() => GetCommand<GetTemperatureCommand>().Execute(true); 
    
    
    /// / <summary>
    ///     Gets the specific Command from the container.
    /// </summary>
    /// <typeparam name="T">TypeOf the Command</typeparam>
    /// <returns></returns>
    private T GetCommand<T>() where T : class, ICommand
    {
        return _serviceProvider.GetService<T>();
    }
}
