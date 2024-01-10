namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 修改密码请求Dto
/// </summary>
public class ChangePasswordRequestDto
{
	/// <summary>
	/// 原密码
	/// </summary>
	public string OldPassword { get; set; }

	/// <summary>
	/// 新密码
	/// </summary>
	public string NewPassword { get; set; }
}