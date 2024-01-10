using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户更新命令
/// </summary>
public sealed class UserUpdateCommand : Command<int, UserUpdateDto>
{
	public UserUpdateCommand(int userId, UserUpdateDto data)
		: base(userId, data)
	{
	}
}