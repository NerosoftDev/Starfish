using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置节点发布命令
/// </summary>
public class SettingPublishCommand : SettingAbstractCommand
{
	public SettingPublishCommand(long appId, string environment)
		: base(appId, environment)
	{
	}
}