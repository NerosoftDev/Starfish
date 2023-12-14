using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 删除配置节点命令
/// </summary>
public class SettingNodeDeleteCommand : Command<long>
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id">配置节点Id</param>
	public SettingNodeDeleteCommand(long id)
		: base(id)
	{
	}
}