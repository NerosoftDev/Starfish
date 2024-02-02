using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 团队成员实体
/// </summary>
public sealed class TeamMember : Entity<string>, IHasCreateTime
{
	private TeamMember()
	{
	}

	private TeamMember(string userId)
		: this()
	{
		UserId = userId;
	}

	public string UserId { get; set; }

	public string TeamId { get; set; }

	public DateTime CreateTime { get; set; }

	public User User { get; set; }

	public Team Team { get; set; }

	internal static TeamMember Create(string userId)
	{
		return new TeamMember(userId);
	}
}