using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNetOnLambda.Services;

public class WeatherService
{
    private readonly HttpClient _http;

    public WeatherService(HttpClient http)
    {
        _http = http;
    }

    public async Task<(string Condition, string Comment)> GetTodayWeatherAsync(string jmaCode)
    {
        var url = $"https://www.jma.go.jp/bosai/forecast/data/forecast/{jmaCode}.json";

        var json = await _http.GetStringAsync(url);

        using var doc = JsonDocument.Parse(json);

        // 今日の天気（weathers[0]）
        var condition = doc.RootElement[0]
            .GetProperty("timeSeries")[0]
            .GetProperty("areas")[0]
            .GetProperty("weathers")[0]
            .GetString();

        if (string.IsNullOrEmpty(condition))
        {
            throw new Exception("気象庁から天気情報を取得できませんでした。");
        }
        
        // 天気に応じてコメントを生成
        var comment = GenerateComment(condition);

        return (condition, comment);
    }

    private string GenerateComment(string condition)
    {
        if (condition.Contains("晴"))
            return "今日は晴れ！お散歩日和だよ。";

        if (condition.Contains("曇"))
            return "今日は曇り。ストールがあると安心だよ。";

        if (condition.Contains("雨"))
            return "今日は雨。傘を忘れないでね。";

        if (condition.Contains("雪"))
            return "雪だよ。暖かくして出かけてね。";

        return "今日も良い一日を！";
    }
}
