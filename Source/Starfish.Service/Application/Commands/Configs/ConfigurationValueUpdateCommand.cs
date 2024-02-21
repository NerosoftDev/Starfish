using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class ConfigurationValueUpdateCommand : Command
{
	public ConfigurationValueUpdateCommand(string id, string key, string value)
	{
		Id = id;
		Key = key;
		Value = value;
	}

	public string Id { get; set; }

	public string Key { get; set; }

	public string Value { get; set; }
}