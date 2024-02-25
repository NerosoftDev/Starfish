using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class ConfigurationDisableCommand : Command
{
	public ConfigurationDisableCommand(string id)
	{
		Id = id;
	}

	public string Id { get; set; }
}