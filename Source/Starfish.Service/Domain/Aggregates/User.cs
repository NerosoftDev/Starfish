using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Common;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户聚合根
/// </summary>
public sealed class User : Aggregate<int>, IHasCreateTime, IHasUpdateTime
{
	/// <summary>
	/// 初始化<see cref="User"/>实例
	/// </summary>
	/// <remarks>
	/// 此构造方法仅提供给EntityFramwork使用
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
	/// <param name="roles"></param>
	private User(string userName, string passwordHash, string passwordSalt, string roles)
		: this()
	{
		UserName = userName;
		PasswordHash = passwordHash;
		PasswordSalt = passwordSalt;
		Roles = roles;
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
	/// 用户角色
	/// </summary>
	public string Roles { get; set; }

	/// <summary>
	/// 创建时间
	/// </summary>
	public DateTime CreateTime { get; set; }

	/// <summary>
	/// 更新时间
	/// </summary>
	public DateTime UpdateTime { get; set; }

	/// <summary>
	/// 新建用户
	/// </summary>
	/// <param name="userName"></param>
	/// <param name="password"></param>
	/// <param name="roles"></param>
	/// <returns></returns>
	internal static User Create(string userName, string password, params string[] roles)
	{
		var salt = RandomUtility.CreateUniqueId();
		var hash = Cryptography.DES.Encrypt(password, Encoding.UTF8.GetBytes(salt));
		var entity = new User(userName, hash, salt, roles.JoinAsString(","));
		return entity;
	}

	/// <summary>
	/// 修改用户信息
	/// </summary>
	/// <param name="nickName"></param>
	/// <param name="roles"></param>
	internal void Update(string nickName, string[] roles)
	{
		NickName = nickName;
		Roles = roles.JoinAsString(",");
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
}