using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class TeamMemberEditCommand : Command
{
	public TeamMemberEditCommand(string teamId, List<string> userIds, string type)
	{
		TeamId = teamId;
		UserIds = userIds;
		Type = type;
	}

	public string TeamId { get; set; }

	public List<string> UserIds { get; set; }

	public string Type { get; set; }
}