namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置节点查询条件
/// </summary>
public class SettingNodeCriteria
{
	/// <summary>
	/// App唯一编码
	/// </summary>
	public string AppCode { get; set; }

	/// <summary>
	/// 环境
	/// </summary>
	public string Environment { get; set; }

	/// <summary>
	/// 上级Id
	/// </summary>
	public long ParentId { get; set; }
}