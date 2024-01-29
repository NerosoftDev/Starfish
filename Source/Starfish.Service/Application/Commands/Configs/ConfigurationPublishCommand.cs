namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置节点发布命令
/// </summary>
public class ConfigurationPublishCommand : ConfigurationAbstractCommand
{
	public ConfigurationPublishCommand(string appId, string environment)
		: base(appId, environment)
	{
	}
}