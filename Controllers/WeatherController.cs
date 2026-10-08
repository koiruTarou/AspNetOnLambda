using Microsoft.AspNetCore.Mvc;
using AspNetOnLambda.Services;
using AspNetOnLambda.Models;

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

    // 例： /weather/230000/230010
     [HttpGet("{prefCode}/{cityCode}")]
    public async Task<IActionResult> Get(string prefCode,string cityCode)
    {
        //天気情報取得
        WeatherInfo info = await _weather.GetTodayWeatherAsync(prefCode,cityCode);
    
        if (info.ResultCd == Common.AppConstants.ResultOK)
        {
            return Ok(info); // 200
        }
        else if (info.ResultCd == Common.AppConstants.ResultErr)
        {
            return BadRequest(info);
        }else
        {
            return StatusCode(500, info); // 500
        }
    }
}
