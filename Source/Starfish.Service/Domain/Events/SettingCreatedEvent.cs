using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public class SettingCreatedEvent : DomainEvent
{
	public SettingCreatedEvent()
	{
	}

	public SettingCreatedEvent(Setting setting)
	{
		Setting = setting;
	}

	public Setting Setting { get; set; }
}