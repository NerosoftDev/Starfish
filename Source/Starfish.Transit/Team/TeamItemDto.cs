namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 团队列表项Dto
/// </summary>
public class TeamItemDto
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
}