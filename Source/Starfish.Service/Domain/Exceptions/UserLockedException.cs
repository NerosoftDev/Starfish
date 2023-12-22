namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户锁定异常
/// </summary>
public class UserLockedException : ForbiddenException
{
	/// <summary>
	/// 初始化<see cref="UserLockedException"/>实例。
	/// </summary>
	public UserLockedException()
		: base(Resources.IDS_ERROR_USER_LOCKOUT)
	{
	}
}