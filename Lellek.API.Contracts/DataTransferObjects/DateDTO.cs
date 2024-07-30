
namespace PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;

public class DateDTO
{
    public string Value { get; set; }

    public int WeekNumber { get; set; }

    public int DayOfYear { get; set; }

    public DateDTO()
    {
        
    }

    public DateDTO(string value, int weekNumber, int dayOfYear)
    {
        this.Value = value;
        this.WeekNumber = weekNumber;
        this.DayOfYear = dayOfYear;
    }
}
