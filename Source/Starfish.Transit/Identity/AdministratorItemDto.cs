namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 管理员列表项数据传输对象
/// </summary>
public sealed class AdministratorItemDto
{
	/// <summary>
	/// 用户Id
	/// </summary>
	public string UserId { get; set; }

	/// <summary>
	/// 用户名
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	/// 昵称
	/// </summary>
	public string NickName { get; set; }

	/// <summary>
	/// 邮箱
	/// </summary>
	public string Email { get; set; }

	/// <summary>
	/// 电话号码
	/// </summary>
	public string Phone { get; set; }

	/// <summary>
	/// 角色
	/// </summary>
	public List<string> Roles { get; set; }
}