using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 令牌刷新事件
/// </summary>
public class TokenRefreshedEvent : ApplicationEvent
{
	/// <summary>
	/// 原始令牌
	/// </summary>
	public string OriginToken { get; set; }
}