
namespace PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;

public class TemperatureDTO
{
    public double Value { get; set; }

    public double Min { get; set; }

    public double Max { get; set; }

    public string Category { get; set; }

    public TemperatureDTO()
    {
        
    }

    public TemperatureDTO(double value, double min, double max, string category)
    {
        this.Value = value;
        this.Min = min;
        this.Max = max;
        this.Category = category;
    }
}
