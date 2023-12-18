using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 新增根节点命令
/// </summary>
public class SettingRootNodeCreateCommand : Command, IQueue<long>
{
	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

	/// <summary>
	/// 环境名称
	/// </summary>
	public string Environment { get; set; }
}