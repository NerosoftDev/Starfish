using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 设置节点Key设置命令
/// </summary>
public class SettingNodeSetKeyCommand : Command<long, string, string>
{
	public SettingNodeSetKeyCommand(long id, string oldName, string newName)
		: base(id, oldName, newName)
	{
	}
}