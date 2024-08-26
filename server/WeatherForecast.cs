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
        var forecasts = Enumerable.Range(1, 50).Select(index => new WeatherForecast
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
}
