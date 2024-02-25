namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置Dto
/// </summary>
public class ConfigurationDto
{
	/// <summary>
	/// Id
	/// </summary>
	public string Id { get; set; }

	/// <summary>
	/// 团队Id
	/// </summary>
	public string TeamId { get; set; }

	/// <summary>
	/// 团队名称
	/// </summary>
	public string TeamName { get; set; }

	/// <summary>
	/// 配置名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 配置描述
	/// </summary>
	public string Description { get; set; }

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
	public string StatusName { get; set; }

	/// <summary>
	/// 修改时间
	/// </summary>
	public DateTime UpdateTime { get; set; }
}