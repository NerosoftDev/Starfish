namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 应用更新请求Dto
/// </summary>
public class AppInfoUpdateDto
{
	/// <summary>
	/// 应用名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 应用描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 是否启用
	/// </summary>
	public bool? IsEnabled { get; set; }
}