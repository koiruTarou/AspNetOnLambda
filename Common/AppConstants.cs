namespace AspNetOnLambda.Common;

public static class AppConstants
{
    // 気象庁 API ベース URL（全体で使う）
    public const string JmaForecastBaseUrl =
        "https://www.jma.go.jp/bosai/forecast/data/forecast/";

    // 共通エラーメッセージ
    public const string ErrorWeatherFetch =
        "気象庁から天気情報を取得できませんでした。時間をおいて再度お試しください。";

    // ログ用メッセージ
    public const string LogWeatherFetchStart = "天気情報取得開始";
    public const string LogWeatherFetchEnd = "天気情報取得終了";

    public const string SunnyMessage = "今日は晴れ！お散歩日和だよ。";
    public const string CloudyMessage = "今日は曇り。ストールがあると安心だよ。";
    public const string RainyMessage = "今日は雨。傘を忘れないでね。";
    public const string SnowMessage = "雪だよ。暖かくして出かけてね。";
    public const string UnknownMessage = "天気情報が取得できなかったよ。時間をおいて試してみてね。";

}
