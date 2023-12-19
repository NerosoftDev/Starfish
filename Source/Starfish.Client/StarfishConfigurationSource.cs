using Microsoft.Extensions.Configuration;

namespace Nerosoft.Starfish.Client;

public class StarfishConfigurationSource : IConfigurationSource
{
	private readonly ConfigurationClientOptions _options;

	public StarfishConfigurationSource(ConfigurationClientOptions options)
	{
		_options = options;
	}

	public IConfigurationProvider Build(IConfigurationBuilder builder)
	{
		return new StarfishConfigurationProvider(_options);
	}
}