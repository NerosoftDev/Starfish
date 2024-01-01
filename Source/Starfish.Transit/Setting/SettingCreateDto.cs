namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置创建Dto
/// </summary>
public class SettingCreateDto
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
	/// 描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 数据类型
	/// </summary>
	/// <remarks>可选值json|text</remarks>
	public string DataType { get; set; }

	/// <summary>
	/// 配置项内容
	/// </summary>
	public string ItemsData { get; set; }
}