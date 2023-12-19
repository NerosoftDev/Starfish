namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置节点详情Dto
/// </summary>
public class SettingNodeDetailDto
{
	/// <summary>
	/// 配置节点Id
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// 父节点Id
	/// </summary>
	/// <remarks>
	/// 0表示根节点
	/// </remarks>
	public long ParentId { get; set; }

	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

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
	/// 配置节点排序
	/// </summary>
	public int Sort { get; set; }
	
	/// <summary>
	/// 配置唯一键
	/// </summary>
	public string Key { get; set; }
}