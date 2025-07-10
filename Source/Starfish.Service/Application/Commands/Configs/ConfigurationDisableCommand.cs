using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class ConfigurationDisableCommand : Command
{
	public ConfigurationDisableCommand(long id)
	{
		Id = id;
	}

	public long Id { get; set; }
}