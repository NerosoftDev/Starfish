using Microsoft.Extensions.Configuration;

namespace Nerosoft.Starfish.Client;

/// <inheritdoc />
public class StarfishConfigurationSource : IConfigurationSource
{
	private readonly ConfigurationClientOptions _options;

	/// <summary>
	/// 初始化<see cref="StarfishConfigurationSource"/>.
	/// </summary>
	/// <param name="options"></param>
	public StarfishConfigurationSource(ConfigurationClientOptions options)
	{
		_options = options;
	}

	/// <inheritdoc />
	public IConfigurationProvider Build(IConfigurationBuilder builder)
	{
		return new StarfishConfigurationProvider(_options);
	}
}