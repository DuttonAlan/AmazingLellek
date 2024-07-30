using Newtonsoft.Json.Linq;
using PlanB.DPF.Manager.Api.Contracts.DataTransferObjects;
using PlanB.DPF.Manager.Core;
using PlanB.PDF.Manager.Service.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlanB.DPF.Manager.Command.Commands.WeatherCommands;

public class GetTemperatureCommand : Command<bool, TemperatureDTO>
{
    private IHttpRequestService requestService;

    public GetTemperatureCommand(IHttpRequestService requestService)
    {
        this.requestService = requestService;
    }

    public override async Task<TemperatureDTO> Execute(bool data)
    {
        dynamic content = await this.requestService.GetContent(HttpMethod.Get, Consts.Temperature_API);

        double[] temperatureArray = ((JArray)content.hourly.temperature_2m).ToObject<double[]>();

        int hour = DateTime.Now.Hour - 1;

        if (temperatureArray.Length == 0)
        {
            throw new Exception("The array has no values.");
        }
        else
        {
            double current = temperatureArray[hour];

            double min = temperatureArray[0];
            double max = temperatureArray[0];

            for (int i = 1; i < temperatureArray.Length; i++)
            {
                double temperature = temperatureArray[i];

                if (temperature < min)
                {
                    min = temperature;
                }

                if (temperature > max)
                {
                    max = temperature;
                }
            }

            int[] weatherCodes = ((JArray)content.hourly.weather_code).ToObject<int[]>();
            string category = GetWeatherByCode(weatherCodes[hour].ToString());

            return new TemperatureDTO(current, min, max, category);
        }
    }

    private static string GetWeatherByCode(string wetterCode) => wetterCode switch
    {
        "0" => "Klar",
        "1" => "Überwiegend",
        "2" => "Teilweise",
        "3" => "Bedeckt",
        "45" => "Nebel",
        "48" => "Reif",
        "51" => "Nieselregen",
        "53" => "Nieselregen",
        "55" => "Nieselregen",
        "56" => "Gefrierender Nieselregen",
        "57" => "Gefrierender Nieselregen",
        "61" => "Regen",
        "63" => "Regen",
        "65" => "Regen",
        "66" => "Gefrierender Regen",
        "67" => "Gefrierender Regen",
        "71" => "Schneefall",
        "73" => "Schneefall",
        "75" => "Schneefall",
        "77" => "Schneekörner",
        "80" => "Regenschauer",
        "81" => "Regenschauer",
        "82" => "Regenschauer",
        "85" => "Schneeschauer",
        "86" => "Schneeschauer",
        "95" => "Gewitter",
        "96" => "Gewitter",
        "99" => "Gewitter",
        _ => "ERROR"
    };

}
