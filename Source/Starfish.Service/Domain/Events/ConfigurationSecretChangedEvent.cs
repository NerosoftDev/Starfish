using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public class ConfigurationSecretChangedEvent : DomainEvent
{
	public ConfigurationSecretChangedEvent(string secret)
	{
		Secret = secret;
	}

	public string Secret { get; set; }
}