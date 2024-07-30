using PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;
using System.Threading.Tasks;
using System.Net.Http;
using PlanB.DPF.Manager.Core;
using PlanB.PDF.Manager.Service.Interfaces;

namespace PlanB.DPF.Manager.Command.Commands.ClockCommands;

public class GetDateCommand : Command<bool, DateDTO>
{
    private IHttpRequestService requestService;

    public GetDateCommand(IHttpRequestService requestService)
    {
        this.requestService = requestService;   
    }


    public async override Task<DateDTO> Execute(bool data) 
    {
        dynamic content = await this.requestService.GetContent(HttpMethod.Get, Consts.Date_API);

        string datetime = content.datetime;
        string date = this.DateConverter(datetime);
        int weekNumber = content.week_number;
        int dayOfYear = content.day_of_year;

        return new DateDTO(date, weekNumber, dayOfYear); 
    }

    private string DateConverter(string datetime)
    {
        string[] datetimeSeperated = datetime.Split(' ');
        string[] dateSeperated = datetimeSeperated[0].Split('/');
        return $"{dateSeperated[2]}-{dateSeperated[1]}-{dateSeperated[0]}";
    } 

}
