using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

internal static class TeamSpecification
{
	public static Specification<Team> IdEquals(long id)
	{
		return new DirectSpecification<Team>(t => t.Id == id);
	}

	public static Specification<Team> AliasEquals(string alias)
	{
		alias = alias.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Team>(t => t.Alias == alias);
	}

	public static Specification<Team> NameEquals(string name)
	{
		name = name.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Team>(t => t.Name.ToLower() == name);
	}

	public static Specification<Team> NameContains(string name)
	{
		name = name.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Team>(t => t.Name.ToLower().Contains(name));
	}

	public static Specification<Team> DescriptionContains(string description)
	{
		description = description.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Team>(t => t.Description.ToLower().Contains(description));
	}

	public static Specification<Team> Matches(string keyword)
	{
		ISpecification<Team>[] specifications =
		[
			NameContains(keyword),
			DescriptionContains(keyword)
		];

		return new CompositeSpecification<Team>(PredicateOperator.OrElse, specifications);
	}

	public static Specification<Team> HasMember(long userId)
	{
		return new DirectSpecification<Team>(t => t.Members.Any(m => m.UserId == userId));
	}
}