using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 用户查询规约
/// </summary>
public static class UserSpecification
{
	/// <summary>
	/// Id等于
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static Specification<User> IdEquals(long id)
	{
		return new DirectSpecification<User>(t => t.Id == id);
	}

	/// <summary>
	/// Id不等于<paramref name="id"/>
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static Specification<User> IdNotEquals(long id)
	{
		return new DirectSpecification<User>(t => t.Id != id);
	}

	/// <summary>
	/// 用户名等于
	/// </summary>
	/// <param name="userName"></param>
	/// <returns></returns>
	public static Specification<User> UserNameEquals(string userName)
	{
		userName = userName.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.UserName == userName);
	}

	public static Specification<User> UserNameContains(string userName)
	{
		userName = userName.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.UserName.Contains(userName));
	}

	public static Specification<User> NickNameContains(string nickName)
	{
		nickName = nickName.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.NickName.ToLower().Contains(nickName));
	}

	/// <summary>
	/// 邮箱等于<paramref name="email"/>
	/// </summary>
	/// <param name="email"></param>
	/// <returns></returns>
	public static Specification<User> EmailEquals(string email)
	{
		email = email.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.Email == email);
	}

	public static Specification<User> EmailContains(string email)
	{
		email = email.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.Email.Contains(email));
	}

	public static Specification<User> HasRole(string role)
	{
		return new DirectSpecification<User>(t => t.Roles.Any(r => r.Name == role));
	}

	public static Specification<User> InRoles(params string[] roles)
	{
		return new DirectSpecification<User>(t => t.Roles.Any(r => roles.Contains(r.Name)));
	}

	public static Specification<User> Matches(string keyword)
	{
		ISpecification<User>[] specifications =
		[
			UserNameContains(keyword),
			NickNameContains(keyword),
			EmailContains(keyword)
		];

		return new CompositeSpecification<User>(PredicateOperator.OrElse, specifications);
	}
}