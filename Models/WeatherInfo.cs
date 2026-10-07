namespace AspNetOnLambda.Models;

public class WeatherInfo
{
    public required string ResultCd{ get; set; } = "1";

    public required string ResultMsg{ get; set; } = "";
    
    public string? PrefCode { get; set; }
    public string? CityCode { get; set; }
    public string? Condition { get; set; } 
    public string? Comment { get; set; }
}
