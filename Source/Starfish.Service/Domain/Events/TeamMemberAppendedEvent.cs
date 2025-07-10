using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

internal sealed class TeamMemberAppendedEvent : DomainEvent
{
	public long UserId { get; set; }
}
