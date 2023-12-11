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
	[Description("启用")]
	Enabled = 1,

	/// <summary>
	/// 禁用
	/// </summary>
	[Description("禁用")]
	Disabled = 2
}