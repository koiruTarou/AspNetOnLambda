namespace AspNetOnLambda.Services;

public static class WeatherConstants
{
    public const string SunnyMessage = "今日は晴れ！お散歩日和だよ。";
    public const string CloudyMessage = "今日は曇り。ストールがあると安心だよ。";
    public const string RainyMessage = "今日は雨。傘を忘れないでね。";
    public const string SnowMessage = "雪だよ。暖かくして出かけてね。";
    public const string UnknownMessage = "天気情報が取得できなかったよ。時間をおいて試してみてね。";

    public const string UnknownStatus = "不明";


    public static readonly Dictionary<string, string> WeatherMap = new()
    {
        { "晴", SunnyMessage },
        { "曇", CloudyMessage },
        { "雨", RainyMessage },
        { "雪", SnowMessage }
    };

}


