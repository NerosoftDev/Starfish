using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户更新命令
/// </summary>
public sealed class UserUpdateCommand : Command<string, UserUpdateDto>
{
	public UserUpdateCommand(string userId, UserUpdateDto data)
		: base(userId, data)
	{
	}
}