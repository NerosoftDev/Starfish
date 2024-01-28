using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

public class ConfigurationArchiveRepository : BaseRepository<DataContext, ConfigurationArchive, long>, IConfigurationArchiveRepository
{
	public ConfigurationArchiveRepository(IContextProvider provider)
		: base(provider)
	{
	}

	public Task<ConfigurationArchive> GetAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		ISpecification<ConfigurationArchive>[] specs =
		[
			ConfigurationArchiveSpecification.AppIdEquals(appId),
			ConfigurationArchiveSpecification.EnvironmentEquals(environment)
		];

		var specification = new CompositeSpecification<ConfigurationArchive>(PredicateOperator.AndAlso, specs);

		var predicate = specification.Satisfy();

		return GetAsync(predicate, true, cancellationToken);
	}
}