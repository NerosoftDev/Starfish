using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

public static class SettingArchiveSpecification
{
	public static Specification<SettingArchive> AppIdEquals(long appId)
	{
		return new DirectSpecification<SettingArchive>(x => x.AppId == appId);
	}

	public static Specification<SettingArchive> EnvironmentEquals(string environment)
	{
		environment = environment.Normalize(TextCaseType.Upper);
		return new DirectSpecification<SettingArchive>(x => x.Environment == environment);
	}
}