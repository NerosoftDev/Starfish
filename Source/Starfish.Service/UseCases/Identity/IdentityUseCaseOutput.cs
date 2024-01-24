using Nerosoft.Euonia.Application;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 身份验证用例输出
/// </summary>
public abstract class IdentityUseCaseOutput : IUseCaseOutput
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
	/// 颁发时间
	/// </summary>
	public DateTime IssuesAt { get; set; }

	/// <summary>
	/// 过期时间
	/// </summary>
	public DateTime ExpiresAt { get; set; }

	/// <summary>
	/// 用户Id
	/// </summary>
	public long UserId { get; set; }
}