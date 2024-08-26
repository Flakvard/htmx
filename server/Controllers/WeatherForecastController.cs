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

        var updatedHtml = WeatherForecastDb.GetHTMLForecast( existingForecast );

        return Content(updatedHtml, "text/html");
    }
    // Update the weather forecast list order
    [HttpPost("order", Name = "OrderWeatherForecasts")]
    public ContentResult Order([FromForm] List<string>? Id)
    {
        _logger.LogInformation("Received order: {Id}", string.Join(", ", Id));

        var forecasts = WeatherForecastDb.GetWeatherForecasts();
        if (Id == null || !Id.Any())
        {
            return Content("No order provided", "text/html");
        }

        var orderedForecasts = WeatherForecastDb.OrderWeatherForecasts(Id, forecasts);
        
        WeatherForecastDb.UpdateWeatherForecast(orderedForecasts);

        var html = WeatherForecastDb.GetHTMLForecastLoop(orderedForecasts);

        return Content(html, "text/html");
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

