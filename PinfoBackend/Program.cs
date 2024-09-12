

using Microsoft.AspNetCore.Mvc;
using PinfoBackend.Cpu;
using Serilog;

namespace PinfoBackend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// Setup serilog
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.WriteTo.File("/log/log-.log", rollingInterval: RollingInterval.Day)
				.CreateLogger();
			
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddAuthorization();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();

			WebApplication app = builder.Build();

			app.UseHttpsRedirection();

			app.UseAuthorization();

			if (Environment.OSVersion.Platform != PlatformID.Unix)
			{
				Log.Error("This API is only supported on Unix.");
				return;
			}

			app.MapGet("/weatherforecast", GetWeatherForecast);
			app.MapGet("/testing", () => "Test answer");
			app.MapGet("/cpuloadpercent", ([FromServices] ICpuManager cpuManager) =>
				{
					return new { Value = cpuManager.GetCpuLoadPercentage() };
				});
			app.MapGet("/cpuarch", () =>
			{
				return new { CpuManager.CpuArchitecture };
			});

			app.Run();
		}

		private static IEnumerable<WeatherForecast> GetWeatherForecast()
		{
			string[] summaries = new[]
			{
				"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
			};

			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
				{
					Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
					TemperatureC = Random.Shared.Next(-20, 55),
					Summary = summaries[Random.Shared.Next(summaries.Length)]
				})
				.ToArray();
		}
	}
}