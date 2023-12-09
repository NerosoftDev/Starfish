using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户创建事件
/// </summary>
public sealed class UserCreatedEvent : DomainEvent
{
	/// <summary>
	/// 用户名
	/// </summary>
	public string UserName { get; set; }
}