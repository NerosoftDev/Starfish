using Microsoft.Extensions.Configuration;
using Nerosoft.Starfish.Client;

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// HostBuilder扩展方法
/// </summary>
public static class HostBuilderExtensions
{
	/// <summary>
	/// 使用Starfish作为配置源
	/// </summary>
	/// <param name="builder"></param>
	/// <param name="optionsAction"></param>
	/// <returns></returns>
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

	/// <summary>
	/// 使用Starfish作为配置源
	/// </summary>
	/// <param name="builder"></param>
	/// <param name="options"></param>
	/// <returns></returns>
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

	/// <summary>
	/// 使用Starfish作为配置源
	/// </summary>
	/// <param name="builder"></param>
	/// <param name="optionsFactory"></param>
	/// <returns></returns>
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
