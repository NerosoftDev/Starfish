namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 登录请求Dto
/// </summary>
public class AuthRequestDto
{
	/// <summary>
	/// 用户名
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	/// 密码
	/// </summary>
	public string Password { get; set; }
}