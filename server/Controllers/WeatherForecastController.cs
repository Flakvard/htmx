using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public ContentResult Get()
    {
        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        var html = "<div class='weather-forecasts'>";
        foreach (var forecast in forecasts)
        {
        html += $"<div class='weather-forecast' onClick=\"this.classList.toggle('selected')\">" +
                $"<p class='date'>Date: {forecast.Date}</p>" +
                $"<p class='temp'>Temperature: {forecast.TemperatureC}°C</p>" +
                $"<p class='summary'>Summary: {forecast.Summary}</p>" +
                $"</div>";
        }
        html += "</div>";

        return Content(html, "text/html");
    }
}

