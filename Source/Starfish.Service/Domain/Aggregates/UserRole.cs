using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户角色
/// </summary>
public class UserRole : Entity<long>
{
	private UserRole()
	{
	}

	private UserRole(string name)
		: this()
	{
		Name = name;
	}

	/// <summary>
	/// 用户Id
	/// </summary>
	public long UserId { get; set; }

	/// <summary>
	/// 角色名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 所属用户
	/// </summary>
	public User User { get; set; }

	internal static UserRole Create(string name)
	{
		return new UserRole(name);
	}
}