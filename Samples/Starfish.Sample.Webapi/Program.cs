using Nerosoft.Starfish.Client;

namespace Starfish.Sample.Webapi;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		// builder.Configuration.AddStarfish(ConfigurationClientOptions.LoadDictionary(new Dictionary<string, string>
		// {
		// 	["Starfish:AppId"] = "starfish.sample",
		// 	["Starfish:AppSecret"] = "0oDjfcWJiO",
		// 	["Starfish:Environment"] = "DEV",
		// 	["Starfish:Host"] = "http://localhost:5229"
		// }));
		//builder.Configuration.AddStarfish(ConfigurationClientOptions.LoadJson($"appsettings.{builder.Environment.EnvironmentName}.json"));

		builder.Configuration.AddStarfish(ConfigurationClientOptions.Load(builder.Configuration));
		
		// Add services to the container.

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}