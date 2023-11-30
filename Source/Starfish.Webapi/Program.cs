using Nerosoft.Euonia.Hosting;
using Nerosoft.Starfish.Webapi;

namespace Starfish.Webapi;

/// <summary>
/// 
/// </summary>
public class Program
{
	/// <summary>
	/// 入口函数
	/// </summary>
	/// <param name="args"></param>
	public static void Main(string[] args)
	{
		static void HostBuilderOptionsAction(HostBuilderOptions options)
		{
			options.EnableHttp2 = true;
			options.UseSerilog = true;
			//options.ConfigureWebHostBuilder = WebHostBuilderAction;
		}

		//static void WebHostBuilderAction(IWebHostBuilder builder)
		//{
		//    builder.ConfigureKestrel(kes =>
		//    {
		//        kes.ListenLocalhost(4567, cfg => cfg.Protocols = HttpProtocols.Http2);
		//        kes.ListenLocalhost(5678, cfg => cfg.Protocols = HttpProtocols.Http1);
		//    });
		//}

		HostUtility.Run<Startup>(args, HostBuilderOptionsAction);

		/*
		var builder = WebApplication.CreateBuilder(args);

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
		*/
	}
}
