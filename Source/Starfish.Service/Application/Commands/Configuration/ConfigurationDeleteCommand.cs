namespace Nerosoft.Starfish.Application;

/// <summary>
/// 删除配置节点命令
/// </summary>
public class ConfigurationDeleteCommand : ConfigurationAbstractCommand
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="environment"></param>
	public ConfigurationDeleteCommand(long id, string environment)
		: base(id, environment)
	{
	}
}