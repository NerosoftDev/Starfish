using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

internal static class ConfigurationSpecification
{
	public static Specification<Configuration> IdEquals(string id)
	{
		return new DirectSpecification<Configuration>(x => x.Id == id);
	}

	public static Specification<Configuration> TeamIdEquals(string appId)
	{
		return new DirectSpecification<Configuration>(x => x.TeamId == appId);
	}

	public static Specification<Configuration> NameEquals(string name)
	{
		name = name.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Configuration>(x => x.Name.ToLower() == name);
	}

	public static Specification<Configuration> NameContains(string name)
	{
		name = name.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Configuration>(x => x.Name.ToLower().Contains(name));
	}

	public static Specification<Configuration> DescriptionContains(string description)
	{
		description = description.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Configuration>(x => x.Description.ToLower().Contains(description));
	}

	public static Specification<Configuration> StatusEquals(ConfigurationStatus status)
	{
		return new DirectSpecification<Configuration>(x => x.Status == status);
	}

	public static Specification<Configuration> Matches(string keyword)
	{
		var specifications = new List<ISpecification<Configuration>>
		{
			NameContains(keyword),
			DescriptionContains(keyword)
		};

		return new CompositeSpecification<Configuration>(PredicateOperator.OrElse, specifications.ToArray());
	}
}