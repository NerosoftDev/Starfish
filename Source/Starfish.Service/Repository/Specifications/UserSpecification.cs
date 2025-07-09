using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 用户查询规约
/// </summary>
internal static class UserSpecification
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
	/// <param name="username"></param>
	/// <returns></returns>
	public static Specification<User> UserNameEquals(string username)
	{
		username = username.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.Username == username);
	}

	public static Specification<User> UserNameContains(string username)
	{
		username = username.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.Username.Contains(username));
	}

	public static Specification<User> NickNameContains(string nickname)
	{
		nickname = nickname.Normalize(TextCaseType.Lower);
		return new DirectSpecification<User>(t => t.Nickname.ToLower().Contains(nickname));
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