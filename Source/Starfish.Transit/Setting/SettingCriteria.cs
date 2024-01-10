namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置节点查询条件
/// </summary>
public class SettingCriteria
{
	/// <summary>
	/// 团队Id
	/// </summary>
	public long TeamId { get; set; }

	/// <summary>
	/// App唯一编码
	/// </summary>
	public string AppCode { get; set; }

	/// <summary>
	/// 环境
	/// </summary>
	public string Environment { get; set; }
}