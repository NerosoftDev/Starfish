using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public class ConfigurationNameChangedEvent : DomainEvent
{
	public ConfigurationNameChangedEvent(string oldName, string newName)
	{
		OldName = oldName;
		NewName = newName;
	}

	public string OldName { get; set; }

	public string NewName { get; set; }
}