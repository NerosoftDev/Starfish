using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置节点发布命令
/// </summary>
public class SettingNodePublishCommand : Command<long>
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	public SettingNodePublishCommand(long id)
		: base(id)
	{
	}
}