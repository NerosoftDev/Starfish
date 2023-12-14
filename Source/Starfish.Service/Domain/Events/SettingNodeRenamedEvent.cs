using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置节点重命名领域事件
/// </summary>
public class SettingNodeRenamedEvent : DomainEvent
{
	/// <summary>
	/// 节点Id
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// 旧名称
	/// </summary>
	public string OldName { get; set; }

	/// <summary>
	/// 新名称
	/// </summary>
	public string NewName { get; set; }
}