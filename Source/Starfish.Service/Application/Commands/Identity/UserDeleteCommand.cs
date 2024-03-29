﻿using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户删除命令
/// </summary>
public sealed class UserDeleteCommand : Command<string>
{
	/// <summary>
	/// 初始化<see cref="UserDeleteCommand"/>实例。
	/// </summary>
	/// <param name="userId"></param>
	public UserDeleteCommand(string userId)
		: base(userId)
	{
	}
}
