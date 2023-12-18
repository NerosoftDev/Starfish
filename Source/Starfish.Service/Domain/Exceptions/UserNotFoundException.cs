namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户未找到异常
/// </summary>
public class UserNotFoundException : NotFoundException
{
	/// <summary>
	/// 初始化<see cref="UserNotFoundException"/>实例。
	/// </summary>
	/// <param name="id"></param>
	public UserNotFoundException(int id)
		: base(string.Format(Resources.IDS_ERROR_USER_NOT_EXISTS, id))
	{
	}
}