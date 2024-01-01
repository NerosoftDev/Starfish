using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置节点发布命令
/// </summary>
public class SettingPublishCommand : Command
{
	public SettingPublishCommand(long id)
	{
		Id = id;
	}

	public long Id { get; }
}