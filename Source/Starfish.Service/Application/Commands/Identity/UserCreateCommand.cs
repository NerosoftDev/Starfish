using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 创建用户命令
/// </summary>
public sealed class UserCreateCommand : Command
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
	/// 电话
	/// </summary>
	public string Phone { get; set; }

	/// <summary>
	/// 昵称
	/// </summary>
	public string NickName { get; set; }

	/// <summary>
	/// 是否是管理员
	/// </summary>
	public bool IsAdmin { get; set; } = false;

	/// <summary>
	/// 是否是预留账号
	/// </summary>
	public bool Reserved { get; set; } = false;
}