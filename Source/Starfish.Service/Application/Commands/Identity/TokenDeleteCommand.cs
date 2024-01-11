using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 删除Token命令
/// </summary>
public class TokenDeleteCommand : Command
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
}