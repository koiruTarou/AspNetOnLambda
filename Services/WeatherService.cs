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

    public async Task<WeatherInfo> GetTodayWeatherAsync(string? prefCode, string? cityCode)
    {

        try
        {
            if (string.IsNullOrEmpty(prefCode) || string.IsNullOrEmpty(cityCode))
            {
                //コード未入力エラーを返す
                return CreateWeather(Common.AppConstants.ResultErr, Common.AppConstants.ErrorNoInputCode, prefCode, cityCode, "", "");
            }

            string url = $"{AppConstants.JmaForecastBaseUrl}{prefCode}.json";

            var json = await _http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);

            string? todayWeather = GetTodayWeather(doc, cityCode);

            if (!string.IsNullOrEmpty(todayWeather))
            {
                //取得成功した天気情報をコメント付きで返す。
                return CreateWeather(Common.AppConstants.ResultOK, "", prefCode, cityCode, todayWeather, GenerateComment(todayWeather));
            }
            else
            {
                //情報取得失敗エラーを返す
                return CreateWeather(Common.AppConstants.ResultErr, Common.AppConstants.ErrorWeatherFetch, prefCode, cityCode, "", "");
            }
        }
        catch (HttpRequestException ex)
        {
            // 気象庁APIサーバーエラー
            return CreateWeather(Common.AppConstants.ResultErr, Common.AppConstants.ErrorServerFailed, prefCode, cityCode, "", "");
        }
        catch(Exception ex)
        {
            //その他の例外
            return CreateWeather(
                Common.AppConstants.ResultErr,
                ex.Message,
                prefCode,
                cityCode,
                "",
                ""
            );
        }
        ;
    }

    public string? GetTodayWeather(JsonDocument doc, string cityCode)
    {
        // 今日・明日・明後日の天気は root[0].timeSeries[0]
        var root0 = doc.RootElement[0];
        var timeSeries0 = root0.GetProperty("timeSeries")[0];
        var areas = timeSeries0.GetProperty("areas");

        foreach (var area in areas.EnumerateArray())
        {
            var code = area.GetProperty("area").GetProperty("code").GetString();
            if (code == cityCode)
            {
                // 市区町村の今日の天気を取得（weathers[0]）
                return area.GetProperty("weathers")[0].GetString();
            }
        }
        return null;
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

    private WeatherInfo CreateWeather(string ResultCd, string ResultMsg, string PrefCode, string CityCode, string Condition, string Comment)
    {
        return new WeatherInfo
        {
            ResultCd = ResultCd,
            ResultMsg = ResultMsg,
            PrefCode = PrefCode,
            CityCode = CityCode,
            Condition = Condition,
            Comment = Comment
        };
    }
}
