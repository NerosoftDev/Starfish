namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 添加团队成员请求Dto
/// </summary>
public class TeamMemberAppendDto
{
	/// <summary>
	/// 用户Id
	/// </summary>
	public string UserId { get; set; }

	/// <summary>
	/// 角色
	/// </summary>
	public string Role { get; set; }
}