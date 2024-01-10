using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

public static class SettingSpecification
{
	public static Specification<Setting> IdEquals(long id)
	{
		return new DirectSpecification<Setting>(x => x.Id == id);
	}

	public static Specification<Setting> AppIdEquals(long appId)
	{
		return new DirectSpecification<Setting>(x => x.AppId == appId);
	}

	public static Specification<Setting> EnvironmentEquals(string environment)
	{
		return new DirectSpecification<Setting>(x => x.Environment == environment);
	}

	public static Specification<Setting> AppCodeEquals(string appCode)
	{
		appCode = appCode.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Setting>(x => x.AppCode == appCode);
	}

	public static Specification<Setting> AppCodeContains(string appCode)
	{
		appCode = appCode.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Setting>(x => x.AppCode.Contains(appCode));
	}

	public static Specification<Setting> StatusEquals(SettingStatus status)
	{
		return new DirectSpecification<Setting>(x => x.Status == status);
	}
}