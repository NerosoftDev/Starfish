using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户认证失败事件
/// </summary>
public class UserAuthFailedEvent : ApplicationEvent
{
	/// <summary>
	/// 认证类型
	/// </summary>
	public string AuthType { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public Dictionary<string, string> Data { get; set; }

	/// <summary>
	/// 错误信息
	/// </summary>
	public string Error { get; set; }
}