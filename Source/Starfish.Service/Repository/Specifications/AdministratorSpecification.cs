using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

internal static class AdministratorSpecification
{
	public static Specification<Administrator> UserNameContains(string userName)
	{
		userName = userName.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Administrator>(t => t.User.UserName.Contains(userName));
	}

	public static Specification<Administrator> NickNameContains(string nickName)
	{
		nickName = nickName.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Administrator>(t => t.User.NickName.ToLower().Contains(nickName));
	}

	public static Specification<Administrator> EmailContains(string email)
	{
		email = email.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Administrator>(t => t.User.Email.Contains(email));
	}

	public static Specification<Administrator> PhoneContains(string email)
	{
		email = email.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Administrator>(t => t.User.Phone.Contains(email));
	}

	public static Specification<Administrator> Matches(string keyword)
	{
		if (string.IsNullOrEmpty(keyword))
		{
			return new TrueSpecification<Administrator>();
		}

		keyword = keyword.Normalize(TextCaseType.Lower);

		ISpecification<Administrator>[] specifications =
		[
			UserNameContains(keyword),
			NickNameContains(keyword),
			EmailContains(keyword),
			PhoneContains(keyword)
		];

		return new CompositeSpecification<Administrator>(PredicateOperator.OrElse, specifications);
	}
}