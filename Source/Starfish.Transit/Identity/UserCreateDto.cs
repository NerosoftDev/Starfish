namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 用户创建Dto
/// </summary>
public class UserCreateDto
{
	/// <summary>
	/// 用户名
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	/// 密码
	/// </summary>
	public string Password { get; set; }

	/// <summary>
	/// 邮箱
	/// </summary>
	public string Email { get; set; }

	/// <summary>
	/// 昵称
	/// </summary>
	public string NickName { get; set; }

	/// <summary>
	/// 用户角色
	/// </summary>
	public List<string> Roles { get; set; }
}