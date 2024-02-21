namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 访问密钥设置请求
/// </summary>
public class ConfigurationSecretSetRequestDto
{
	/// <summary>
	/// 访问密钥
	/// </summary>
	public string Secret { get; set; }
}