namespace server;

public class WeatherForecast
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
    public bool Selected { get; set; } = false;
}

public static class WeatherForecastDb
{
    public static List<WeatherForecast> WeatherForecasts { get; set; } = new();
   public static void AddWeatherForecast(WeatherForecast forecast)
    {
        WeatherForecasts.Add(forecast);
    }
    public static void UpdateWeatherForecast(WeatherForecast forecast)
    {
        var index = WeatherForecasts.FindIndex(f => f.Id == forecast.Id);
        if (index != -1)
        {
            WeatherForecasts[index] = forecast;
        }
    }
    // Update the weather forecast list order
    public static void UpdateWeatherForecast(List<WeatherForecast> forecasts)
    {
        WeatherForecasts = forecasts;
    }
    public static void DeleteWeatherForecast(Guid id)
    {
        var index = WeatherForecasts.FindIndex(f => f.Id == id);
        if (index != -1)
        {
            WeatherForecasts.RemoveAt(index);
        }
    }
    public static List<WeatherForecast> GetWeatherForecasts()
    {
        return WeatherForecasts;
    }
    public static WeatherForecast GetWeatherForecast(Guid id)
    {
        return WeatherForecasts.Find(f => f.Id == id);
    }
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    public static void SeedWeatherForecasts()
    {
        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Id = Guid.NewGuid(),
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToList();
        WeatherForecasts = forecasts;
    }
    private static WeatherForecast[] RandWeatherForecasts()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();
    }
    // Search for the weather forecast by any property
    public static List<WeatherForecast> SearchWeatherForecasts(string search)
    {
        return WeatherForecasts.Where(f =>
            f.Date.ToString().Contains(search) ||
            f.TemperatureC.ToString().Contains(search) ||
            f.TemperatureF.ToString().Contains(search) ||
            f.Summary?.Contains(search) == true ||
            f.Selected.ToString().Contains(search)
        ).ToList();
    }
    // Update the weather forecast list order
    public static List<WeatherForecast> OrderWeatherForecasts(List<string> order, List<WeatherForecast> forecasts)
    {
        var orderedForecasts = new List<WeatherForecast>();
        foreach (var id in order)
        {
            // var parts = id.Split('=');
            // string forecastId = parts.Length > 1 ? parts[1] : parts[0];
            // var forecast = forecasts.Find(f => f.Id.ToString() == forecastId);
            var forecast = forecasts.Find(f => f.Id.ToString() == id);
            if (forecast != null)
            {
                orderedForecasts.Add(forecast);
            }
        }
        return orderedForecasts;
    }
    
    public static string GetHTMLForForecast(List<WeatherForecast> forecasts)
    {
        var html = "<div id='weather-forecasts' class='col rounded border border-1 p-5 m-3 justify-content-center'>";
        html += GetHTMLForecastLoop(forecasts);
        html += "</div>";
        return html;
    }
    public static string GetHTMLForecastLoop(List<WeatherForecast> forecasts)
    {
        var html = $"<form class='sortable' "+
                $"hx-post='http://localhost:5146/weatherforecast/order' " +
                $"hx-trigger='end'> " +
                $"<div class='htmx-indicator'>Loading...</div>";
        foreach (var forecast in forecasts)
        {
            html += "<div class='row m-1 p-3 border-1'>";
            html += $"<div id='weather-forecast' class='pe-auto m-3 p-3 border-1 rounded shadow {(forecast.Selected ? "bg-success" : "bg-secondary")}' data-id='{forecast.Id}'" +
                    $"hx-put='http://localhost:5146/weatherforecast/{forecast.Id}' " +
                    $"hx-trigger='click' " +
                    $"hx-swap='outerHTML'>" +
                    $"<input type='hidden' name='Id' value='{forecast.Id}' />" +
                    $"<p class='user-select-none'>Date: {forecast.Date}</p>" +
                    $"<p class='user-select-none'>Temperature: {forecast.TemperatureC}°C</p>" +
                    $"<p class='user-select-none'>Summary: {forecast.Summary}</p>" +
                    $"<p class='user-select-none'>Selected: {forecast.Selected}</p>" +
                    $"</div>";
            html += "</div>";
        }
        html += "</form>";
        return html;
    }
    public static string GetHTMLForecast(WeatherForecast forecast)
    {
        var html = $"<div id='weather-forecast' class='m-3 p-3 rounded shadow {(forecast.Selected ? "bg-success" : "bg-secondary")}' data-id='{forecast.Id}'" +
                $"hx-put='http://localhost:5146/weatherforecast/{forecast.Id}' " +
                $"hx-trigger='click' " +
                $"hx-swap='outerHTML'>" +
                $"<p class='date'>Date: {forecast.Date}</p>" +
                $"<p class='temp'>Temperature: {forecast.TemperatureC}°C</p>" +
                $"<p class='summary'>Summary: {forecast.Summary}</p>" +
                $"<p class='selected'>Selected: {forecast.Selected}</p>" +
                $"</div>";
        html += "</div>";
        return html;
    }
}
