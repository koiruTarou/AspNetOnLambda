namespace AspNetOnLambda.Common;

public static class AppConstants
{
    // 気象庁 API ベース URL（全体で使う）
    public const string JmaForecastBaseUrl =
        "https://www.jma.go.jp/bosai/forecast/data/forecast/";

    public const string ResultErr = "1";
    public const string ResultOK = "0";

    public const string ErrorNoInputCode = "気象情報を取得するためのコードが入力されていません";
    public const string ErrorWeatherFetch = "気象庁から天気情報を取得できませんでした。入力したコードが間違っていないか確認してください";

}
