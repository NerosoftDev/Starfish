﻿using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户聚合根
/// </summary>
public sealed class User : Aggregate<string>, IHasCreateTime, IHasUpdateTime, ITombstone
{
	/// <summary>
	/// 初始化<see cref="User"/>实例
	/// </summary>
	/// <remarks>
	/// 此构造方法仅提供给EntityFramework使用
	/// </remarks>
	private User()
	{
	}

	/// <summary>
	/// 初始化用户聚合根
	/// </summary>
	/// <param name="userName"></param>
	/// <param name="password"></param>
	/// 
	private User(string userName, string password)
		: this()
	{
		UserName = userName;
		SetPassword(password);
	}

	/// <summary>
	/// 用户名
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	/// 密码加密后的哈希字符串
	/// </summary>
	public string PasswordHash { get; set; }

	/// <summary>
	/// 加密密码使用的盐
	/// </summary>
	public string PasswordSalt { get; set; }

	/// <summary>
	/// 昵称
	/// </summary>
	public string NickName { get; set; }

	/// <summary>
	/// 邮箱
	/// </summary>
	public string Email { get; set; }

	/// <summary>
	/// 电话
	/// </summary>
	public string Phone { get; set; }

	/// <summary>
	/// 授权失败次数
	/// </summary>
	public int AccessFailedCount { get; set; }

	/// <summary>
	/// 锁定结束时间
	/// </summary>
	public DateTime? LockoutEnd { get; set; }

	/// <summary>
	/// 是否是预留账号
	/// </summary>
	/// <remarks>预留账号不允许删除、设置角色等</remarks>
	public bool Reserved { get; set; }

	/// <summary>
	/// 是否是管理员
	/// </summary>
	public bool IsAdmin { get; set; }

	/// <summary>
	/// 来源
	/// </summary>
	public int Source { get; set; }

	/// <summary>
	/// 创建时间
	/// </summary>
	public DateTime CreateTime { get; set; }

	/// <summary>
	/// 更新时间
	/// </summary>
	public DateTime UpdateTime { get; set; }

	/// <summary>
	/// 是否被删除
	/// </summary>
	public bool IsDeleted { get; set; }

	/// <summary>
	/// 删除时间
	/// </summary>
	public DateTime? DeleteTime { get; set; }

	/// <summary>
	/// 新建用户
	/// </summary>
	/// <param name="userName"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	internal static User Create(string userName, string password)
	{
		var entity = new User(userName, password);
		return entity;
	}

	/// <summary>
	/// 设置密码
	/// </summary>
	/// <param name="password"></param>
	/// <param name="actionType"></param>
	internal void SetPassword(string password, string actionType = null)
	{
		var salt = RandomUtility.GenerateUniqueId();
		var hash = Cryptography.DES.Encrypt(password, Encoding.UTF8.GetBytes(salt));
		PasswordHash = hash;
		PasswordSalt = salt;
		if (!string.IsNullOrWhiteSpace(actionType))
		{
			RaiseEvent(new UserPasswordChangedEvent { Type = actionType });
		}
	}

	/// <summary>
	/// 设置邮箱
	/// </summary>
	/// <param name="email"></param>
	internal void SetEmail(string email)
	{
		Email = email.Normalize(TextCaseType.Lower);
	}

	/// <summary>
	/// 设置电话
	/// </summary>
	/// <param name="phone"></param>
	internal void SetPhone(string phone)
	{
		Phone = phone;
	}

	/// <summary>
	/// 设置用户昵称
	/// </summary>
	/// <param name="nickName"></param>
	internal void SetNickName(string nickName)
	{
		NickName = nickName;
	}

	internal void SetIsAdmin(bool isAdmin)
	{
		if (Reserved)
		{
			return;
		}

		if (isAdmin == IsAdmin)
		{
			return;
		}

		IsAdmin = isAdmin;
	}

	/// <summary>
	/// 增加授权失败次数
	/// </summary>
	internal void IncreaseAccessFailedCount()
	{
		AccessFailedCount++;
		if (AccessFailedCount >= 10)
		{
			LockoutEnd = DateTime.Now.AddMinutes(30);
		}
	}

	/// <summary>
	/// 重置授权失败次数
	/// </summary>
	internal void ResetAccessFailedCount()
	{
		AccessFailedCount = 0;
		LockoutEnd = null;
	}
}