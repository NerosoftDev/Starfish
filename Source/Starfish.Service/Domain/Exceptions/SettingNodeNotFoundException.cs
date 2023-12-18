namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置信息不存在异常
/// </summary>
public class SettingNodeNotFoundException : NotFoundException
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	public SettingNodeNotFoundException(long id)
		: base(string.Format(Resources.IDS_ERROR_SETTING_NODE_NOT_EXISTS, id))
	{
	}
}