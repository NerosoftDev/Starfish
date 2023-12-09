namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 用户认证响应Dto
/// </summary>
public class AuthResponseDto
{
	/// <summary>
	/// 访问令牌
	/// </summary>
	public string AccessToken { get; set; }

	/// <summary>
	/// 刷新令牌
	/// </summary>
	public string RefreshToken { get; set; }

	/// <summary>
	/// 令牌类型
	/// </summary>
	public string TokenType { get; set; }

	/// <summary>
	/// 令牌过期时间
	/// </summary>
	public long ExpiresIn { get; set; }
}