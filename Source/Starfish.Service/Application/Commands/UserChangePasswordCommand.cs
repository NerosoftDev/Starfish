using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户修改密码命令
/// </summary>
public class UserChangePasswordCommand : Command
{
	/// <summary>
	/// 用户Id
	/// </summary>
	public int UserId { get; set; }

	/// <summary>
	/// 新密码
	/// </summary>
	public string Password { get; set; }
}
