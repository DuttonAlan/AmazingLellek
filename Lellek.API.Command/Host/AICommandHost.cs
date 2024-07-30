using Microsoft.Extensions.DependencyInjection;
using PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;
using PlanB.DPF.Manager.Command.Commands;
using PlanB.DPF.Manager.Command.Commands.AICommands;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlanB.DPF.Manager.Command.Host;

public partial class AICommandHost
{
    private readonly IServiceProvider _serviceProvider;

    public AICommandHost(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<string> ConvertMp3ToText(byte[] mp3File) => GetCommand<SpeechToTextCommand>().Execute(mp3File);

    public Task<string> GenerateTextAnswer(string text) => GetCommand<GenerateAnswerCommand>().Execute(text);

    public Task<HttpResponseMessage> GenerateSpeechAnswer(string text) => GetCommand<TextToSpeechCommand>().Execute(text);

    private T GetCommand<T>() where T : class, ICommand
    {
        return _serviceProvider.GetService<T>();
    }
}
//test
