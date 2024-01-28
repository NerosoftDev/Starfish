using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

public static class ConfigurationArchiveSpecification
{
	public static Specification<ConfigurationArchive> AppIdEquals(long appId)
	{
		return new DirectSpecification<ConfigurationArchive>(x => x.AppId == appId);
	}

	public static Specification<ConfigurationArchive> EnvironmentEquals(string environment)
	{
		environment = environment.Normalize(TextCaseType.Upper);
		return new DirectSpecification<ConfigurationArchive>(x => x.Environment == environment);
	}
}