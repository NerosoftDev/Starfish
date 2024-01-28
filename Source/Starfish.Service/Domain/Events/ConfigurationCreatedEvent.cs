using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public class ConfigurationCreatedEvent : DomainEvent
{
	public ConfigurationCreatedEvent()
	{
	}

	public ConfigurationCreatedEvent(Configuration configuration)
	{
		Configuration = configuration;
	}

	public Configuration Configuration { get; set; }
}