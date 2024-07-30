
namespace PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;

public class TimeDTO
{
    public string Value { get; set; }

    public string TimeZone { get; set; }

    public TimeDTO()
    {
        
    }

    public TimeDTO(string value, string timeZone)
    {
        this.Value = value;
        this.TimeZone = timeZone;
    }
}

