namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置节点更新Dto
/// </summary>
public class SettingNodeUpdateDto
{
	/// <summary>
	/// 名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 配置值
	/// </summary>
	public string Value { get; set; }

	/// <summary>
	/// 配置描述
	/// </summary>
	/// <remarks>
	///	仅在更新节点描述时有效
	/// </remarks>
	public string Description { get; set; }
}