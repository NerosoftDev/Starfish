namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 团队详情Dto
/// </summary>
public class TeamDetailDto
{
	/// <summary>
	/// Id
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// 别名
	/// </summary>
	public string Alias { get; set; }

	/// <summary>
	/// 名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 团队描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 团队所有者Id
	/// </summary>
	public int OwnerId { get; set; }

	/// <summary>
	/// 成员数量
	/// </summary>
	public int MemberCount { get; set; }

	/// <summary>
	/// 创建时间
	/// </summary>
	public DateTime CreateTime { get; set; }

	/// <summary>
	/// 更新时间
	/// </summary>
	public DateTime UpdateTime { get; set; }

	/// <summary>
	/// 创建人
	/// </summary>
	public string CreatedBy { get; set; }

	/// <summary>
	/// 更新人
	/// </summary>
	public string UpdatedBy { get; set; }
}