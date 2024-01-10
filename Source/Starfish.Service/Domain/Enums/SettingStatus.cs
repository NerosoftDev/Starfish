using System.ComponentModel;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置节点状态
/// </summary>
public enum SettingStatus
{
	/// <summary>
	/// 
	/// </summary>
	None = 0,

	/// <summary>
	/// 待发布
	/// </summary>
	[Description(nameof(Resources.IDS_ENUM_SETTING_STATUS_PENDING))]
	Pending = 1,

	/// <summary>
	/// 已发布
	/// </summary>
	[Description(nameof(Resources.IDS_ENUM_SETTING_STATUS_PUBLISHED))]
	Published = 2,

	/// <summary>
	/// 禁用
	/// </summary>
	[Description(nameof(Resources.IDS_ENUM_SETTING_STATUS_DISABLED))]
	Disabled = 3
}
