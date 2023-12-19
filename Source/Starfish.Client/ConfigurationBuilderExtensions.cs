﻿using Nerosoft.Starfish.Client;

namespace Microsoft.Extensions.Configuration;

public static class ConfigurationBuilderExtensions
{
	public static IConfigurationBuilder AddStarfish(this IConfigurationBuilder builder, Action<ConfigurationClientOptions> optionsAction)
	{
		var options = new ConfigurationClientOptions();
		optionsAction(options);
		builder.Add(new StarfishConfigurationSource(options));
		return builder;
	}

	public static IConfigurationBuilder AddStarfish(this IConfigurationBuilder builder, ConfigurationClientOptions options)
	{
		builder.Add(new StarfishConfigurationSource(options));
		return builder;
	}

	public static IConfigurationBuilder AddStarfish(this IConfigurationBuilder builder, Func<IConfigurationBuilder, ConfigurationClientOptions> optionsFactory)
	{
		ArgumentNullException.ThrowIfNull(optionsFactory);
		var options = optionsFactory(builder);
		builder.Add(new StarfishConfigurationSource(options));
		return builder;
	}
}