using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 团队成员实体
/// </summary>
public sealed class TeamMember : Entity<long>, IHasCreateTime
{
	private TeamMember()
	{
	}

	private TeamMember(long userId)
		: this()
	{
		UserId = userId;
	}

	public long UserId { get; set; }

	public long TeamId { get; set; }

	public DateTime CreateTime { get; set; }

	public User User { get; set; }

	public Team Team { get; set; }

	internal static TeamMember Create(long userId)
	{
		return new TeamMember(userId);
	}
}