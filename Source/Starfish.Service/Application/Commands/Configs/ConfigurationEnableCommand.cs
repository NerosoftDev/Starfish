using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class ConfigurationEnableCommand : Command
{
	public ConfigurationEnableCommand(long id)
	{
		Id = id;
	}

	public long Id { get; set; }
}