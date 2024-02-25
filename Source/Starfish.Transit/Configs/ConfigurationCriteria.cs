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
	/// 关键字
	/// </summary>
	public string Keyword { get; set; }

	/// <summary>
	/// 状态
	/// </summary>
	public int Status { get; set; }
}