using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class TeamMemberEditCommand : Command
{
	public TeamMemberEditCommand(int teamId, List<int> userIds, string type)
	{
		TeamId = teamId;
		UserIds = userIds;
		Type = type;
	}

	public int TeamId { get; set; }

	public List<int> UserIds { get; set; }

	public string Type { get; set; }
}