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

	public static Specification<Setting> StatusEquals(SettingStatus status)
	{
		return new DirectSpecification<Setting>(x => x.Status == status);
	}

	public static Specification<SettingItem> SettingAppIdEquals(long appId)
	{
		return new DirectSpecification<SettingItem>(x => x.Setting.AppId == appId);
	}

	public static Specification<SettingItem> SettingAppEnvironmentEquals(string environment)
	{
		return new DirectSpecification<SettingItem>(x => x.Setting.Environment == environment);
	}
}