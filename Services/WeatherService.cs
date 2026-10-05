using System.Net.Http;
using System.Text.Json;
using AspNetOnLambda.Common;
using AspNetOnLambda.Models;

namespace AspNetOnLambda.Services;

public class WeatherService
{
    private readonly HttpClient _http;

    public WeatherService(HttpClient http)
    {
        _http = http;

        // Lambda で TLS1.2 を強制
        System.Net.ServicePointManager.SecurityProtocol =
            System.Net.SecurityProtocolType.Tls12;
    }

    public async Task<WeatherInfo> GetTodayWeatherAsync(string jmaCode)
    {
        var url = $"{AppConstants.JmaForecastBaseUrl}{jmaCode}.json";

        var json = await _http.GetStringAsync(url);
        using var doc = JsonDocument.Parse(json);

        // weathers を持つ timeSeries を探す
        JsonElement? weatherSeries = null;

        foreach (var ts in doc.RootElement[0].GetProperty("timeSeries").EnumerateArray())
        {
            if (ts.TryGetProperty("areas", out var areas) &&
                areas[0].TryGetProperty("weathers", out var _))
            {
                weatherSeries = ts;
                break;
            }
        }

        if (weatherSeries == null)
        {
            return CreateErrorInfo(); 
        }

        var area = weatherSeries.Value.GetProperty("areas")[0];
        var city = area.GetProperty("area").GetProperty("name").GetString();
        var condition = area.GetProperty("weathers")[0].GetString();

        if (city == null || condition == null)
        {
            return CreateErrorInfo();
        }else{
            return new WeatherInfo
            {
                City = city,
                Condition = condition,
                Comment = GenerateComment(condition)
            };
        }
    }

     private string GenerateComment(string condition)
    {
        foreach (var kv in WeatherConstants.WeatherMap)
        {
            //天気情報に応じて、コメント返却
            if (condition.Contains(kv.Key))
                return kv.Value;
        }

        return WeatherConstants.UnknownMessage;
    }

    private WeatherInfo CreateErrorInfo()
    {
        return new WeatherInfo
        {
            City = WeatherConstants.UnknownStatus,
            Condition =  WeatherConstants.UnknownStatus,
            Comment = WeatherConstants.UnknownMessage
        };
    }
}
