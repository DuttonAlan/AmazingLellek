using System.Threading.Tasks;

namespace PlanB.DPF.Manager.Command.Commands;

/// <summary>
/// The Command abstract class
/// </summary>
public abstract class Command<TInput, TOutput> : ICommand
{
    public abstract Task<TOutput> Execute(TInput data);
}
