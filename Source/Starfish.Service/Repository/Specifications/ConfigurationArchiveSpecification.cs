using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

internal static class ConfigurationArchiveSpecification
{
	public static Specification<ConfigurationArchive> TeamIdEquals(string teamId)
	{
		return new DirectSpecification<ConfigurationArchive>(x => x.Configuration.TeamId == teamId);
	}

	public static Specification<ConfigurationArchive> NameEquals(string name)
	{
		name = name.Normalize(TextCaseType.Upper);
		return new DirectSpecification<ConfigurationArchive>(x => x.Configuration.Name == name);
	}
}