using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 团队聚合根对象
/// </summary>
public sealed class Team : Aggregate<string>,
						   IAuditing
{
	/// <summary>
	/// 团队名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 团队描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 团队负责人Id
	/// </summary>
	public string OwnerId { get; set; }

	/// <summary>
	/// 团队成员数
	/// </summary>
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
		team.AppendMember(ownerId);
		team.RaiseEvent(new TeamCreatedEvent());
		return team;
	}

	/// <summary>
	/// 设置团队名称
	/// </summary>
	/// <param name="name"></param>
	internal void SetName(string name)
	{
		if (string.Equals(Name, name))
		{
			return;
		}

		Name = name;
	}

	/// <summary>
	/// 设置团队描述
	/// </summary>
	/// <param name="description"></param>
	internal void SetDescription(string description)
	{
		if (string.Equals(Description, description))
		{
			return;
		}

		Description = description;
	}

	/// <summary>
	/// 添加团队成员
	/// </summary>
	/// <param name="userId"></param>
	internal void AppendMember(string userId)
	{
		Members ??= [];

		if (Members.Any(t => t.UserId == userId))
		{
			return;
		}

		Members.Add(TeamMember.Create(userId));

		MemberCount++;

		if (!string.IsNullOrEmpty(Id))
		{
			RaiseEvent(new TeamMemberAppendedEvent { UserId = userId });
		}
	}

	/// <summary>
	///	移除团队成员
	/// </summary>
	/// <param name="userId"></param>
	/// <exception cref="InvalidOperationException"></exception>
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

		RaiseEvent(new TeamMemberRemovedEvent { UserId = userId });
	}
}