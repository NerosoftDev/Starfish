namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 应用创建请求Dto
/// </summary>
public class AppInfoCreateDto
{
	/// <summary>
	/// 团队Id
	/// </summary>
	public string TeamId { get; set; }

	/// <summary>
	/// 应用名称
	/// </summary>
	public string Name { get; set; } = default!;

	/// <summary>
	/// 应用代码
	/// </summary>
	public string Code { get; set; } = default!;

	/// <summary>
	/// 密钥
	/// </summary>
	public string Secret { get; set; }

	/// <summary>
	/// 应用描述
	/// </summary>
	public string Description { get; set; }
}