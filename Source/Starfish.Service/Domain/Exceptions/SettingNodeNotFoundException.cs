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
		: base($"Setting node with id {id} not found.")
	{
	}
}