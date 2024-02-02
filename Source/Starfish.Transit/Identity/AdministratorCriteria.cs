namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 管理员搜索条件
/// </summary>
public class AdministratorCriteria
{
	/// <summary>
	/// 关键字
	/// </summary>
	/// <remarks>
	/// 搜索用户名、昵称、邮箱、电话号码
	/// </remarks>
	public string Keyword { get; set; }
}