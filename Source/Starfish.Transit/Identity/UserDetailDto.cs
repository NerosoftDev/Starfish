﻿namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 用户详情Dto
/// </summary>
public class UserDetailDto
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

	/// <summary>
	/// 创建时间
	/// </summary>
	public DateTime CreateTime { get; set; }

	/// <summary>
	/// 更新时间
	/// </summary>
	public DateTime UpdateTime { get; set; }
}