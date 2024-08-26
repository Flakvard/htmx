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

        var html = WeatherForecastDb.GetHTMLForForecast(forecasts);

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

        var updatedHtml = WeatherForecastDb.GetHTMLForecastLoop(new List<WeatherForecast> { existingForecast });

        return Content(updatedHtml, "text/html");
    }
    // search for the weather forecast by any property
    [HttpPost("search", Name = "SearchWeatherForecasts")]
    public ContentResult Search([FromForm] string? search)
    {
        var forecasts = new List<WeatherForecast>();
        if (string.IsNullOrEmpty(search))
        {
            forecasts = WeatherForecastDb.GetWeatherForecasts();

        }else{
            forecasts = WeatherForecastDb.SearchWeatherForecasts(search);
        }
        var html = WeatherForecastDb.GetHTMLForForecast(forecasts);

        return Content(html, "text/html");
    }
        

}

