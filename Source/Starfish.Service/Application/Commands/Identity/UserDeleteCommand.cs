﻿using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户删除命令
/// </summary>
public sealed class UserDeleteCommand : Command<long>
{
	/// <summary>
	/// 初始化<see cref="UserDeleteCommand"/>实例。
	/// </summary>
	/// <param name="userId"></param>
	public UserDeleteCommand(long userId)
		: base(userId)
	{
	}
}
