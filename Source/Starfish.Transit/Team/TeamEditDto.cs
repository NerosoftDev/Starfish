namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 团队编辑Dto
/// </summary>
public class TeamEditDto
{
	/// <summary>
	/// 别名
	/// </summary>
	/// <remarks>全局唯一</remarks>
	public string Alias { get; set; }

	/// <summary>
	/// 团队名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 团队描述
	/// </summary>
	public string Description { get; set; }
}