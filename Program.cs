using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

app.MapWeatherRoutes();
app.MapStarWarsRoutes();

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

	public string? SecondCharacter { get; set; }
}