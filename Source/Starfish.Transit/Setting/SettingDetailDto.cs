namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置详情Dto
/// </summary>
public class SettingDetailDto
{
	/// <summary>
	/// Id
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

	/// <summary>
	/// 应用唯一编码
	/// </summary>
	public string AppCode { get; set; }

	/// <summary>
	/// 应用名称
	/// </summary>
	public string AppName { get; set; }

	/// <summary>
	/// 环境名称
	/// </summary>
	public string Environment { get; set; }

	/// <summary>
	/// 当前版本号
	/// </summary>
	public string Version { get; set; }

	/// <summary>
	/// 状态
	/// </summary>
	public string Status { get; set; }

	/// <summary>
	/// 状态名称
	/// </summary>
	public string StatusDescription { get; set; }
}