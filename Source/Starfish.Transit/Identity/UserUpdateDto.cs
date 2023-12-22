namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 用户编辑Dto
/// </summary>
public class UserUpdateDto
{
	/// <summary>
	/// 邮箱
	/// </summary>
	public string Email { get; set; }

	/// <summary>
	/// 昵称
	/// </summary>
	public string NickName { get; set; }

	/// <summary>
	/// 角色
	/// </summary>
	public List<string> Roles { get; set; }
}
