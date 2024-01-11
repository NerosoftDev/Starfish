using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 删除配置节点命令
/// </summary>
public class SettingDeleteCommand : SettingAbstractCommand
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="environment"></param>
	public SettingDeleteCommand(long id, string environment)
		: base(id, environment)
	{
	}
}