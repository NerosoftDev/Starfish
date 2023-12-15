using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

public static class SettingArchiveSpecification
{
	public static Specification<SettingArchive> AppCodeEquals(string appCode)
	{
		appCode = appCode.Normalize(TextCaseType.Lower);
		return new DirectSpecification<SettingArchive>(x => x.AppCode == appCode);
	}

	public static Specification<SettingArchive> EnvironmentEquals(string environment)
	{
		environment = environment.Normalize(TextCaseType.Upper);
		return new DirectSpecification<SettingArchive>(x => x.Environment == environment);
	}
}