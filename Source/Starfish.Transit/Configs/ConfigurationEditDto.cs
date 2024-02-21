namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置更新Dto
/// </summary>
public class ConfigurationEditDto
{
	/// <summary>
	/// 配置名称
	/// </summary>
	/// <remarks>
	///	同一个TeamId下的配置名称不能重复
	/// </remarks>
	public string Name { get; set; }

	/// <summary>
	/// 配置描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 访问密钥
	/// </summary>
	public string Secret { get; set; }
}