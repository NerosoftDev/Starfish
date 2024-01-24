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

	public ChangePasswordCommand(long userId, string password)
		: this()
	{
		UserId = userId;
		Password = password;
	}

	/// <summary>
	/// 用户Id
	/// </summary>
	public long UserId { get; set; }

	/// <summary>
	/// 新密码
	/// </summary>
	public string Password { get; set; }
}