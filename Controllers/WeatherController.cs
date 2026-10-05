using Microsoft.AspNetCore.Mvc;
using AspNetOnLambda.Services;

namespace AspNetOnLambda.Controllers;

[ApiController]
[Route("weather")]
public class WeatherController : ControllerBase
{
    private readonly WeatherService _weather;

    public WeatherController(WeatherService weather)
    {
        _weather = weather;
    }

    // 例： /weather/230000
    [HttpGet("{jmaCode}")]
    public async Task<IActionResult> Get(string jmaCode)
    {
        var (condition, comment) = await _weather.GetTodayWeatherAsync(jmaCode);

        return Ok(new { condition, comment });
    }
}
