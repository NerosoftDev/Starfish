using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 团队聚合根对象
/// </summary>
public sealed class Team : Aggregate<string>,
                           IAuditing
{
	public string Name { get; set; }

	public string Description { get; set; }

	public string OwnerId { get; set; }

	public int MemberCount { get; set; }

	public DateTime CreateTime { get; set; }

	public DateTime UpdateTime { get; set; }

	public string CreatedBy { get; set; }

	public string UpdatedBy { get; set; }

	public HashSet<TeamMember> Members { get; set; }

	internal static Team Create(string name, string description, string ownerId)
	{
		var team = new Team
		{
			Name = name,
			Description = description,
			OwnerId = ownerId
		};
		team.AddMember(ownerId);
		return team;
	}

	internal void SetName(string name)
	{
		if (string.Equals(Name, name))
		{
			return;
		}

		Name = name;
	}

	internal void SetDescription(string description)
	{
		if (string.Equals(Description, description))
		{
			return;
		}

		Description = description;
	}

	internal void AddMember(string userId)
	{
		Members ??= [];

		if (Members.Any(t => t.UserId == userId))
		{
			return;
		}

		Members.Add(TeamMember.Create(userId));

		MemberCount++;
	}

	internal void RemoveMember(string userId)
	{
		if (Members == null || Members.All(t => t.UserId != userId))
		{
			return;
		}

		if (userId == OwnerId)
		{
			throw new InvalidOperationException(Resources.IDS_ERROR_TEAM_OWNER_CANNOT_REMOVED);
		}

		Members.RemoveWhere(t => t.UserId == userId);

		MemberCount--;
	}
}