using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置节点状态变更领域事件
/// </summary>
public class SettingStatusChangedEvent : DomainEvent
{
	public SettingStatusChangedEvent()
	{
	}

	public SettingStatusChangedEvent(SettingStatus oldStatus, SettingStatus newStatus)
	{
		OldStatus = oldStatus;
		NewStatus = newStatus;
	}

	public SettingStatus OldStatus { get; set; }

	public SettingStatus NewStatus { get; set; }
}