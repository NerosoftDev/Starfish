using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class TeamMemberEditCommand : Command
{
	public TeamMemberEditCommand(long teamId, List<long> userIds, string type)
	{
		TeamId = teamId;
		UserIds = userIds;
		Type = type;
	}

	public long TeamId { get; set; }

	public List<long> UserIds { get; set; }

	public string Type { get; set; }
}