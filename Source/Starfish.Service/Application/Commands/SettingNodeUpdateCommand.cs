using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 更新配置节点命令
/// </summary>
public class SettingNodeUpdateCommand : Command<long, SettingNodeUpdateDto>
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	public SettingNodeUpdateCommand(long id, SettingNodeUpdateDto data)
		: base(id, data)
	{
	}
}