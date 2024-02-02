namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置节点查询条件
/// </summary>
public class ConfigurationCriteria
{
	/// <summary>
	/// 团队Id
	/// </summary>
	public string TeamId { get; set; }

	/// <summary>
	/// App唯一编码
	/// </summary>
	public string AppName { get; set; }

	/// <summary>
	/// 应用环境
	/// </summary>
	public string Environment { get; set; }
}