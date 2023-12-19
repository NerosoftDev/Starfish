using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Starfish.Client;

namespace Microsoft.Extensions.Hosting;

public static class HostBuilderExtensions
{
	public static IHostBuilder UseStarfish(this IHostBuilder builder, Action<ConfigurationClientOptions> optionsAction)
	{
		builder.ConfigureAppConfiguration((context, config) =>
		{
			config.AddStarfish(optionsAction);
		});
		// builder.ConfigureServices((context, services) =>
		// {
		// 	services.AddHostedService<StarfishConfigurationService>();
		// });
		return builder;
	}

	public static IHostBuilder UseStarfish(this IHostBuilder builder, ConfigurationClientOptions options)
	{
		builder.ConfigureAppConfiguration((context, config) =>
		{
			config.AddStarfish(options);
		});
		// builder.ConfigureServices((context, services) =>
		// {
		// 	services.AddHostedService<StarfishConfigurationService>();
		// });
		return builder;
	}

	public static IHostBuilder UseStarfish(this IHostBuilder builder, Func<IConfigurationBuilder, ConfigurationClientOptions> optionsFactory)
	{
		builder.ConfigureAppConfiguration((context, config) =>
		{
			config.AddStarfish(optionsFactory);
		});
		// builder.ConfigureServices((context, services) =>
		// {
		// 	services.AddHostedService<StarfishConfigurationService>();
		// });
		return builder;
	}
}
