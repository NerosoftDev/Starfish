using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户修改密码命令
/// </summary>
public class ChangePasswordCommand : Command
{
	public ChangePasswordCommand()
	{
	}

	public ChangePasswordCommand(string userId, string password)
		: this()
	{
		UserId = userId;
		Password = password;
	}

	/// <summary>
	/// 用户Id
	/// </summary>
	public string UserId { get; set; }

	/// <summary>
	/// 新密码
	/// </summary>
	public string Password { get; set; }
}