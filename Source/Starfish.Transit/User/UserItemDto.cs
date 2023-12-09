namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 用户查询结果Dto
/// </summary>
public class UserItemDto
{
	/// <summary>
	/// Id
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// 用户名
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	/// 邮箱
	/// </summary>
	public string Email { get; set; }

	/// <summary>
	/// 昵称
	/// </summary>
	public string NickName { get; set; }

	/// <summary>
	/// 用户角色
	/// </summary>
	public List<string> Roles { get; set; }
}