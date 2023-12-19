namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置节点Dto
/// </summary>
public class SettingNodeItemDto
{
	/// <summary>
	/// 配置节点Id
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// 应用唯一编码
	/// </summary>
	public string AppCode { get; set; }

	/// <summary>
	/// 环境名称
	/// </summary>
	public string Environment { get; set; }

	/// <summary>
	/// 配置名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 配置值
	/// </summary>
	public string Value { get; set; }

	/// <summary>
	/// 配置描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 配置节点类型
	/// </summary>
	public string Type { get; set; }

	/// <summary>
	/// 是否是根节点
	/// </summary>
	public string IsRoot { get; set; }

	/// <summary>
	/// 配置唯一键
	/// </summary>
	public string Key { get; set; }
}