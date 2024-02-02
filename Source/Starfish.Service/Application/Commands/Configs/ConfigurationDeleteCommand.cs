namespace Nerosoft.Starfish.Application;

/// <summary>
/// 删除配置节点命令
/// </summary>
public class ConfigurationDeleteCommand : ConfigurationAbstractCommand
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="appId">应用Id</param>
	/// <param name="environment"></param>
	public ConfigurationDeleteCommand(string appId, string environment)
		: base(appId, environment)
	{
	}
}