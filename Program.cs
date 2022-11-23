using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<Settings>()
	.BindConfiguration(nameof(Settings));

builder.Services.AddLogging(options =>
{
	options.AddConsole();
	options.AddDebug();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
	"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (IOptionsSnapshot<Settings> settings, ILoggerFactory logFactory) =>
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
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal record struct WeatherForecastDto(DateTime Date, int Temperature, string? Summary);

internal enum TemperatureUnit
{
	Celsius,
	Fahrenheit
}

internal class Settings
{
	public TemperatureUnit TemperatureUnit { get; set; }
}