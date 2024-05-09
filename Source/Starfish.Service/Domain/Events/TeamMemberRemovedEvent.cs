using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

internal class TeamMemberRemovedEvent : DomainEvent
{
	public string UserId { get; set; }
}
