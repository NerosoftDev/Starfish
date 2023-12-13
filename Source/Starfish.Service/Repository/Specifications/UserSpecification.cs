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
	public static Specification<User> IdEquals(int id)
	{
		return new DirectSpecification<User>(t => t.Id == id);
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
}