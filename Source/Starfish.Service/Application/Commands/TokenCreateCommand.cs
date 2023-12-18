using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 
/// </summary>
public class TokenCreateCommand : Command
{
	/// <summary>
	/// Token类型
	/// </summary>
	/// <remarks>
	///	可选值：access_token, refresh_token
	/// </remarks>
	public string Type { get; set; }

	/// <summary>
	/// Token原文
	/// </summary>
	public string Token { get; set; }

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
}