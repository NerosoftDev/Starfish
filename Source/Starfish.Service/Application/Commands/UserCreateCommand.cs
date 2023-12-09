using Nerosoft.Euonia.Domain;

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
	/// 用户密码
	/// </summary>
	public string Password { get; set; }

	/// <summary>
	/// 邮箱
	/// </summary>
	public string Email { get; set; }

	/// <summary>
	/// 用户角色
	/// </summary>
	public string[] Roles { get; set; }
}