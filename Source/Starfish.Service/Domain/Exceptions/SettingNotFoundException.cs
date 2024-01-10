namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置信息不存在异常
/// </summary>
public class SettingNotFoundException : NotFoundException
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	public SettingNotFoundException(long id)
		: base(string.Format(Resources.IDS_ERROR_SETTING_NOT_EXISTS, id))
	{
	}
}