using Nerosoft.Euonia.Core;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户未找到异常
/// </summary>
public class UserNotFoundException : NotFoundException
{
	/// <summary>
	/// 初始化<see cref="UserNotFoundException"/>实例。
	/// </summary>
	/// <param name="userId"></param>
	public UserNotFoundException(int userId)
		: base($"用户未找到，用户Id：{userId}")
	{
	}
}