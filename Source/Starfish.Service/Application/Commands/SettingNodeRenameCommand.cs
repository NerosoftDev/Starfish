using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 重命名配置节点命令
/// </summary>
public class SettingNodeRenameCommand : Command<long, string>
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	/// <param name="name"></param>
	public SettingNodeRenameCommand(long id, string name)
		: base(id, name)
	{
	}
}