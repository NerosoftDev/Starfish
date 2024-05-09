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

	public ChangePasswordCommand(string userId, string password, string actionType)
		: this()
	{
		UserId = userId;
		Password = password;
		ActionType = actionType;
	}

	/// <summary>
	/// 用户Id
	/// </summary>
	public string UserId { get; set; }

	/// <summary>
	/// 新密码
	/// </summary>
	public string Password { get; set; }

	/// <summary>
	/// 操作方式
	/// </summary>
	/// <value>change-修改；reset-重置</value>
	public string ActionType { get; set; }
}