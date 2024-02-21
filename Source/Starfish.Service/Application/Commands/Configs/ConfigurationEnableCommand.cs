using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class ConfigurationEnableCommand : Command
{
	public ConfigurationEnableCommand(string id)
	{
		Id = id;
	}

	public string Id { get; set; }
}