

namespace PinfoBackend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// Setup serilog
			// Log.Logger = new LoggerConfiguration()
			// 	.WriteTo.Console()
			// 	.WriteTo.File("/log/log-.log", rollingInterval: RollingInterval.Day)
			// 	.CreateLogger();
			
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddAuthorization();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();

			var app = builder.Build();

			app.UseHttpsRedirection();

			app.UseAuthorization();

			// if (Environment.OSVersion.Platform != PlatformID.Unix)
			// {
			// 	Log.Error("This API is only supported on Unix.");
			// 	return;
			// }

			var summaries = new[]
			{
				"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
			};

			app.MapGet("/weatherforecast", (HttpContext httpContext) =>
				{
					var forecast = Enumerable.Range(1, 5).Select(index =>
							new WeatherForecast
							{
								Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
								TemperatureC = Random.Shared.Next(-20, 55),
								Summary = summaries[Random.Shared.Next(summaries.Length)]
							})
						.ToArray();
					return forecast;
				})
				.WithName("GetWeatherForecast");

			app.Run();
		}
	}
}