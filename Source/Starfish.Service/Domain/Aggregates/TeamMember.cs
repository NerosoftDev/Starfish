using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 团队成员实体
/// </summary>
public sealed class TeamMember : Entity<int>, IHasCreateTime
{
	private TeamMember()
	{
	}

	private TeamMember(int userId)
		: this()
	{
		UserId = userId;
	}

	public int UserId { get; set; }

	public int TeamId { get; set; }

	public DateTime CreateTime { get; set; }

	public User User { get; set; }

	public Team Team { get; set; }

	internal static TeamMember Create(int userId)
	{
		return new TeamMember(userId);
	}
}