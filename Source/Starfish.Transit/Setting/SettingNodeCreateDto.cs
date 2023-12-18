namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 创建配置节点（根节点）
/// </summary>
public class SettingNodeCreateDto
{
	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

	/// <summary>
	/// 环境名称
	/// </summary>
	public string Environment { get; set; }
	
	/// <summary>
	/// 节点名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 节点配置值
	/// </summary>
	public string Value { get; set; }

	/// <summary>
	/// 节点描述
	/// </summary>
	public string Description { get; set; }
}