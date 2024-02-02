namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 管理员编辑数据传输对象
/// </summary>
public class AdministratorAssignDto
{
	/// <summary>
	/// 用户Id
	/// </summary>
	public string UserId { get; set; }

	/// <summary>
	/// 角色
	/// </summary>
	public List<string> Roles { get; set; }
}
