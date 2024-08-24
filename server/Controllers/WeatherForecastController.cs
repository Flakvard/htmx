using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public ContentResult Get()
    {
        var forecasts = WeatherForecastDb.GetWeatherForecasts();

        var html = "<div class='weather-forecasts'>";
        foreach (var forecast in forecasts)
        {
            html += $"<div class='weather-forecast {(forecast.Selected ? "selected" : "")}' data-id='{forecast.Id}' " +
                    $"hx-put='http://localhost:5146/weatherforecast/{forecast.Id}' " +
                    $"hx-trigger='click' " +
                    $"hx-swap='outerHTML'>" +
                    $"<p class='date'>Date: {forecast.Date}</p>" +
                    $"<p class='temp'>Temperature: {forecast.TemperatureC}°C</p>" +
                    $"<p class='summary'>Summary: {forecast.Summary}</p>" +
                    $"<p class='selected'>Selected: {forecast.Selected}</p>" +
                    $"</div>";
        }
        html += "</div>";

        return Content(html, "text/html");
    }
    [HttpPut("{id}", Name = "UpdateWeatherForecast")]
    public IActionResult Put(Guid id)
    {
        var existingForecast = WeatherForecastDb.GetWeatherForecast(id);
        if (existingForecast == null)
        {
            return NotFound();
        }

        existingForecast.Selected = !existingForecast.Selected; // Toggle the selected state

        WeatherForecastDb.UpdateWeatherForecast(existingForecast);

        var updatedHtml = $"<div class='weather-forecast {(existingForecast.Selected ? "selected" : "")}' data-id='{existingForecast.Id}' " +
                        $"hx-put='http://localhost:5146/weatherforecast/{existingForecast.Id}' " +
                        $"hx-trigger='click' " +
                        $"hx-swap='outerHTML'>" +
                        $"<p class='date'>Date: {existingForecast.Date}</p>" +
                        $"<p class='temp'>Temperature: {existingForecast.TemperatureC}°C</p>" +
                        $"<p class='summary'>Summary: {existingForecast.Summary}</p>" +
                        $"<p class='selected'>Selected: {existingForecast.Selected}</p>" +
                        $"</div>";

        return Content(updatedHtml, "text/html");
    }

}

