using Microsoft.AspNetCore.Hosting.Server.Features;
using Nerosoft.Euonia.Hosting;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Starfish.Application;
using Serilog;

namespace Nerosoft.Starfish.Webapi;

/// <summary>
/// 托管服务模块上下文
/// </summary>
[DependsOn(typeof(HostingModule), typeof(ApplicationServiceModule))]
public class HostServiceModule : ModuleContextBase
{
	/// <inheritdoc/>
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		context.Services.AddLogging(builder =>
		{
			builder.AddConfiguration(Configuration.GetSection("Logging"))
			       .AddConsole()
			       .AddDebug()
			       .AddSerilog();
		});
		context.Services.AddHealthChecks();
		context.Services.AddControllers();
		context.Services.AddAuthentication(Configuration);
		context.Services.AddSwagger();
	}

	/// <inheritdoc />
	public override void OnApplicationInitialization(ApplicationInitializationContext context)
	{
		var app = context.GetApplicationBuilder();
		app.ServerFeatures.Get<IServerAddressesFeature>();

		app.UseSerilogRequestLogging();
		app.UseForwardedHeaders();
		app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); //.AllowCredentials());
	}
}