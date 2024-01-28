namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置更新Dto
/// </summary>
public class ConfigurationEditDto
{
	/// <summary>
	/// 数据类型
	/// </summary>
	public string Type { get; set; }

	/// <summary>
	/// 配置内容
	/// </summary>
	public string Data { get; set; }
}