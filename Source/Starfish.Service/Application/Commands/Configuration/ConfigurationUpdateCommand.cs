namespace Nerosoft.Starfish.Application;

public class ConfigurationUpdateCommand : ConfigurationAbstractCommand
{
	public ConfigurationUpdateCommand(long appId, string environment)
		: base(appId, environment)
	{
		AppId = appId;
		Environment = environment;
	}

	public IDictionary<string, string> Data { get; set; }
}