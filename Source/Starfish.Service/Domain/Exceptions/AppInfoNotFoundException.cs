namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用信息不存在异常
/// </summary>
public class AppInfoNotFoundException : NotFoundException
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	public AppInfoNotFoundException(long id)
		: base(string.Format(Resources.IDS_ERROR_APPINFO_NOT_EXISTS, id))
	{
	}
}