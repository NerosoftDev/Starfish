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

	private TeamMember(string userId)
		: this()
	{
		UserId = userId;
	}

	/// <summary>
	/// 用户Id
	/// </summary>
	public string UserId { get; set; }

	/// <summary>
	/// 团队Id
	/// </summary>
	public string TeamId { get; set; }

	/// <summary>
	/// 创建时间
	/// </summary>
	public DateTime CreateTime { get; set; }

	public User User { get; set; }

	public Team Team { get; set; }

	internal static TeamMember Create(string userId)
	{
		return new TeamMember(userId);
	}
}