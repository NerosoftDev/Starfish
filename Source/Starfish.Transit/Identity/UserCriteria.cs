namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 用户查询条件
/// </summary>
public class UserCriteria
{
	/// <summary>
	/// 关键字
	/// </summary>
	/// <remarks>模糊查询用户/邮箱</remarks>
	public string Keyword { get; set; }
}