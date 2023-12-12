using System.ComponentModel;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用状态枚举
/// </summary>
public enum AppStatus
{
	/// <summary>
	/// 
	/// </summary>
	None = 0,

	/// <summary>
	/// 启用
	/// </summary>
	[Description(nameof(Resources.IDS_ENUM_APPSTATUS_ENABLED))]
	Enabled = 1,

	/// <summary>
	/// 禁用
	/// </summary>
	[Description(nameof(Resources.IDS_ENUM_APPSTATUS_DISABLED))]
	Disabled = 2
}