using IdentityModel;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 令牌
/// </summary>
public sealed class Token : Entity<long>
{
	/// <summary>
	/// 初始化<see cref="Token"/>实例。
	/// </summary>
	private Token()
	{
	}

	/// <summary>
	/// 初始化<see cref="Token"/>实例。
	/// </summary>
	/// <param name="type"></param>
	/// <param name="key"></param>
	/// <param name="subject"></param>
	/// <param name="issues"></param>
	/// <param name="expires"></param>
	private Token(string type, string key, string subject, DateTime issues, DateTime? expires = null)
		: this()
	{
		Type = type;
		Key = key;
		Subject = subject;
		Expires = expires;
		Issues = issues;
	}

	/// <summary>
	/// Token类型
	/// </summary>
	/// <remarks>
	///	可选值：access_token, refresh_token
	/// </remarks>
	public string Type { get; set; }

	/// <summary>
	/// Token经过SHA256加密后的值
	/// </summary>
	public string Key { get; set; }

	/// <summary>
	/// 用户ID
	/// </summary>
	public string Subject { get; set; }

	/// <summary>
	/// 颁发时间
	/// </summary>
	public DateTime Issues { get; set; }

	/// <summary>
	/// 过期时间
	/// </summary>
	public DateTime? Expires { get; set; }

	/// <summary>
	/// 创建新的<see cref="Token"/>实例。
	/// </summary>
	/// <param name="type">Token类型</param>
	/// <param name="token">Token内容</param>
	/// <param name="subject">用户Id</param>
	/// <param name="issues">颁发时间</param>
	/// <param name="expires">过期时间</param>
	/// <returns></returns>
	internal static Token Create(string type, string token, string subject, DateTime issues, DateTime? expires = null)
	{
		var key = token.ToSha256();
		return new Token(type, key, subject, issues, expires);
	}
}