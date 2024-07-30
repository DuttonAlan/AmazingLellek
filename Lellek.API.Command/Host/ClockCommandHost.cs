using Microsoft.Extensions.DependencyInjection;
using PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;
using PlanB.DPF.Manager.Command.Commands;
using PlanB.DPF.Manager.Command.Commands.ClockCommands;
using System;
using System.Threading.Tasks;

namespace PlanB.DPF.Manager.Command.Host;

public partial class ClockCommandHost
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="serviceProvider"></param>
    public ClockCommandHost(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TimeDTO> GetTime() => GetCommand<GetTimeCommand>().Execute(true); 

    public Task<DateDTO> GetDate() => GetCommand<GetDateCommand>().Execute(true); 


    /// <summary>
    ///     Gets the specific Command from the container.
    /// </summary>
    /// <typeparam name="T">TypeOf the Command</typeparam>
    /// <returns></returns>
    private T GetCommand<T>() where T : class, ICommand 
    {
        return _serviceProvider.GetService<T>(); 
    }
}
