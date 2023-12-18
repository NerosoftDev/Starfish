using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置节点创建领域事件
/// </summary>
public class SettingNodeCreatedEvent : DomainEvent
{
	/// <summary>
	/// 构造函数
	/// </summary>
	public SettingNodeCreatedEvent()
	{
	}

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="node"></param>
	public SettingNodeCreatedEvent(SettingNode node)
	{
		Node = node;
	}

	/// <summary>
	/// 配置节点
	/// </summary>
	public SettingNode Node { get; set; }
}