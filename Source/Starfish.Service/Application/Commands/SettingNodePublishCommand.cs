using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置节点发布命令
/// </summary>
public class SettingNodePublishCommand : Command
{
	public SettingNodePublishCommand(long id)
	{
		Id = id;
	}

	public long Id { get; }
}