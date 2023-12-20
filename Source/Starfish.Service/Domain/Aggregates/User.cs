using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Common;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户聚合根
/// </summary>
public sealed class User : Aggregate<int>, IHasCreateTime, IHasUpdateTime, ITombstone
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
	/// <param name="passwordHash"></param>
	/// <param name="passwordSalt"></param>
	private User(string userName, string passwordHash, string passwordSalt)
		: this()
	{
		UserName = userName;
		PasswordHash = passwordHash;
		PasswordSalt = passwordSalt;
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
	/// 用户角色
	/// </summary>
	public HashSet<UserRole> Roles { get; set; }

	/// <summary>
	/// 新建用户
	/// </summary>
	/// <param name="userName"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	internal static User Create(string userName, string password)
	{
		var salt = RandomUtility.CreateUniqueId();
		var hash = Cryptography.DES.Encrypt(password, Encoding.UTF8.GetBytes(salt));
		var entity = new User(userName, hash, salt);
		return entity;
	}

	/// <summary>
	/// 修改密码
	/// </summary>
	/// <param name="password"></param>
	internal void ChangePassword(string password)
	{
		var salt = RandomUtility.CreateUniqueId();
		var hash = Cryptography.DES.Encrypt(password, Encoding.UTF8.GetBytes(salt));
		PasswordHash = hash;
		PasswordSalt = salt;
	}

	/// <summary>
	/// 设置用户角色
	/// </summary>
	/// <param name="roles"></param>
	internal void SetRoles(params string[] roles)
	{
		if (Reserved)
		{
			throw new UnauthorizedAccessException("预留账号不允许设置角色");
		}

		Roles ??= [];
		roles ??= [];
		Roles.RemoveWhere(t => roles.Contains(t.Name, StringComparison.OrdinalIgnoreCase));
		foreach (var role in roles)
		{
			Roles.Add(UserRole.Create(role));
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
	/// 设置用户昵称
	/// </summary>
	/// <param name="nickName"></param>
	internal void SetNickName(string nickName)
	{
		NickName = nickName;
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