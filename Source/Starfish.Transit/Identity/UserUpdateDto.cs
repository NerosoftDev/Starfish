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
	/// 电话号码
	/// </summary>
	public string Phone { get; set; }

	/// <summary>
	/// 昵称
	/// </summary>
	public string NickName { get; set; }

	/// <summary>
	/// 是否是管理员
	/// </summary>
	public bool IsAdmin { get; set; }
}
