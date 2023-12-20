using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 创建用户命令
/// </summary>
public sealed class UserCreateCommand : Command<UserCreateDto>
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="data">用户创建数据传输对象</param>
	public UserCreateCommand(UserCreateDto data)
		: base(data)
	{
	}
}