using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

public static class ConfigurationSpecification
{
	public static Specification<Configuration> IdEquals(long id)
	{
		return new DirectSpecification<Configuration>(x => x.Id == id);
	}

	public static Specification<Configuration> AppIdEquals(string appId)
	{
		return new DirectSpecification<Configuration>(x => x.AppId == appId);
	}

	public static Specification<Configuration> EnvironmentEquals(string environment)
	{
		return new DirectSpecification<Configuration>(x => x.Environment == environment);
	}

	public static Specification<Configuration> StatusEquals(ConfigurationStatus status)
	{
		return new DirectSpecification<Configuration>(x => x.Status == status);
	}

	public static Specification<ConfigurationItem> ConfigurationAppIdEquals(string appId)
	{
		return new DirectSpecification<ConfigurationItem>(x => x.Configuration.AppId == appId);
	}

	public static Specification<ConfigurationItem> ConfigurationAppEnvironmentEquals(string environment)
	{
		return new DirectSpecification<ConfigurationItem>(x => x.Configuration.Environment == environment);
	}
}