namespace Nerosoft.Starfish.Application;

public class ConfigurationValueUpdateCommand : ConfigurationAbstractCommand
{
	public ConfigurationValueUpdateCommand(long appId, string environment, string key, string value)
		: base(appId, environment)
	{
		AppId = appId;
		Environment = environment;
		Key = key;
		Value = value;
	}

	public string Key { get; set; }

	public string Value { get; set; }
}