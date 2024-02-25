using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class ConfigurationUpdateCommand : Command
{
	public ConfigurationUpdateCommand(string id)
	{
		Id = id;
	}

	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Secret { get; set; }
}