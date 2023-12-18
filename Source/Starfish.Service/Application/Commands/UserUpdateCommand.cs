using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户更新命令
/// </summary>
public sealed class UserUpdateCommand : Command
{
	/// <summary>
	/// 用户Id
	/// </summary>
	public int UserId { get; set; }

	/// <summary>
	/// 昵称
	/// </summary>
	public string NickName { get; set; }

	/// <summary>
	/// 邮箱
	/// </summary>
	public string Email { get; set; }

	/// <summary>
	/// 角色
	/// </summary>
	public string[] Roles { get; set; }
}