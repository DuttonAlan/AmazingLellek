using PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;
using PlanB.DPF.Manager.Core;
using PlanB.PDF.Manager.Service.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlanB.DPF.Manager.Command.Commands.ClockCommands;

public class GetTimeCommand : Command<bool, TimeDTO>
{
    private IHttpRequestService requestService;

    public GetTimeCommand(IHttpRequestService requestService)
    {
        this.requestService = requestService; 
    }
    
    public async override Task<TimeDTO> Execute(bool data) 
    {
        dynamic content = await this.requestService.GetContent(HttpMethod.Get, Consts.Time_API); 

        string time = content.time;
        string timeZone = content.timeZone; 

        return new TimeDTO(time, timeZone); 
    }
}
