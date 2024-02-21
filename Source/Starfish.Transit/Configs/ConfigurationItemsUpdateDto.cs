namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 配置项更新数据传输对象
/// </summary>
public class ConfigurationItemsUpdateDto
{
	/// <summary>
	/// 数据类型
	/// </summary>
	public string Type { get; set; }

	/// <summary>
	/// 更新方式
	/// </summary>
	/// <remarks>
	///	<para>diff-差异更新</para>
	/// <para>full-全量更新</para>
	/// </remarks>
	public string Mode { get; set; }

	/// <summary>
	/// 数据
	/// </summary>
	/// <remarks>
	///	使用Base64编码
	/// </remarks>
	public string Data { get; set; }
}