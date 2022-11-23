using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public static class WeatherRoutes
{
  private static string[] summaries = new[]
  {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
  };

  private const string GroupName = "GetWeatherForecast";

  public static void MapWeatherRoutes(this IEndpointRouteBuilder routeBuilder)
  {
    routeBuilder.MapGet("/weatherforecast", (IOptionsSnapshot<Settings> settings, ILoggerFactory logFactory) =>
    {
      var logger = logFactory.CreateLogger("WeatherForecast");
      logger.LogInformation("Getting weather forecast in {TemperatureUnit} unit", settings.Value.TemperatureUnit.ToString());
      return Enumerable.Range(1, 5)
        .Select(index =>
          new WeatherForecast
          (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
          ))
        .Select(m => settings switch 
        {
          {Value: {TemperatureUnit: TemperatureUnit.Fahrenheit}} =>
            new WeatherForecastDto(m.Date, m.TemperatureC, m.Summary),
          _ =>
            new WeatherForecastDto(m.Date, m.TemperatureF, m.Summary),
        })
        .ToArray();
    })
    .WithName(GroupName);
  }
}