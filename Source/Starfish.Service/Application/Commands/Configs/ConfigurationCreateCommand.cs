using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class ConfigurationCreateCommand : Command
{
	public ConfigurationCreateCommand(string teamId)
	{
		TeamId = teamId;
	}

	public string TeamId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Secret { get; set; }
}