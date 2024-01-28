using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置节点状态变更领域事件
/// </summary>
public class ConfigurationStatusChangedEvent : DomainEvent
{
	public ConfigurationStatusChangedEvent()
	{
	}

	public ConfigurationStatusChangedEvent(ConfigurationStatus oldStatus, ConfigurationStatus newStatus)
	{
		OldStatus = oldStatus;
		NewStatus = newStatus;
	}

	public ConfigurationStatus OldStatus { get; set; }

	public ConfigurationStatus NewStatus { get; set; }
}