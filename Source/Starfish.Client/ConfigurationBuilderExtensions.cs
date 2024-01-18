using Nerosoft.Starfish.Client;

// ReSharper disable MemberCanBePrivate.Global

namespace Microsoft.Extensions.Configuration;

/// <summary>
/// 配置构建器扩展方法
/// </summary>
public static class ConfigurationBuilderExtensions
{
	/// <summary>
	/// 添加Starfish作为配置源
	/// </summary>
	/// <param name="manager"></param>
	/// <returns></returns>
	public static ConfigurationManager AddStarfish(this ConfigurationManager manager)
	{
		manager.AddStarfish(manager);
		return manager;
	}

	/// <summary>
	/// 添加Starfish作为配置源
	/// </summary>
	/// <param name="builder"></param>
	/// <param name="configuration"></param>
	/// <returns></returns>
	public static IConfigurationBuilder AddStarfish(this IConfigurationBuilder builder, IConfiguration configuration)
	{
		return builder.AddStarfish(ConfigurationClientOptions.Load(configuration));
	}

	/// <summary>
	/// 添加Starfish作为配置源
	/// </summary>
	/// <param name="builder"></param>
	/// <param name="optionsAction"></param>
	/// <returns></returns>
	public static IConfigurationBuilder AddStarfish(this IConfigurationBuilder builder, Action<ConfigurationClientOptions> optionsAction)
	{
		var options = new ConfigurationClientOptions();
		optionsAction(options);
		builder.Add(new StarfishConfigurationSource(options));
		return builder;
	}

	/// <summary>
	/// 添加Starfish作为配置源
	/// </summary>
	/// <param name="builder"></param>
	/// <param name="options"></param>
	/// <returns></returns>
	public static IConfigurationBuilder AddStarfish(this IConfigurationBuilder builder, ConfigurationClientOptions options)
	{
		builder.Add(new StarfishConfigurationSource(options));
		return builder;
	}

	/// <summary>
	/// 添加Starfish作为配置源
	/// </summary>
	/// <param name="builder"></param>
	/// <param name="optionsFactory"></param>
	/// <returns></returns>
	public static IConfigurationBuilder AddStarfish(this IConfigurationBuilder builder, Func<IConfigurationBuilder, ConfigurationClientOptions> optionsFactory)
	{
		ArgumentNullException.ThrowIfNull(optionsFactory);
		var options = optionsFactory(builder);
		builder.Add(new StarfishConfigurationSource(options));
		return builder;
	}
}