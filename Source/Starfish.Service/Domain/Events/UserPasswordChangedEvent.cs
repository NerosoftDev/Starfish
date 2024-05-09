using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户密码被修改
/// </summary>
public sealed class UserPasswordChangedEvent : DomainEvent
{
	/// <summary>
	/// 操作类型
	/// </summary>
	/// <remarks>
	/// - change
	/// - reset
	/// </remarks>
	public string Type { get; set; }
}