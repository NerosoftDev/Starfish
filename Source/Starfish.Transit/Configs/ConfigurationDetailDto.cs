namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置详情Dto
/// </summary>
public class ConfigurationDetailDto
{
	/// <summary>
	/// Id
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// 应用Id
	/// </summary>
	public string AppId { get; set; }

	/// <summary>
	/// 应用名称
	/// </summary>
	public string AppName { get; set; }

	/// <summary>
	/// 应用环境
	/// </summary>
	public string Environment { get; set; }

	/// <summary>
	/// 当前版本号
	/// </summary>
	public string Version { get; set; }

	/// <summary>
	/// 发布时间
	/// </summary>
	public DateTime? PublishTime { get; set; }

	/// <summary>
	/// 状态
	/// </summary>
	public string Status { get; set; }

	/// <summary>
	/// 状态名称
	/// </summary>
	public string StatusDescription { get; set; }

	/// <summary>
	/// 修改时间
	/// </summary>
	public DateTime UpdateTime { get; set; }
}