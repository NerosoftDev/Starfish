using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

public class ConfigurationArchiveRepository : BaseRepository<DataContext, ConfigurationArchive, string>, IConfigurationArchiveRepository
{
	public ConfigurationArchiveRepository(IContextProvider provider)
		: base(provider)
	{
	}

	public Task<ConfigurationArchive> GetAsync(string teamId, string name, CancellationToken cancellationToken = default)
	{
		ISpecification<ConfigurationArchive>[] specs =
		[
			ConfigurationArchiveSpecification.TeamIdEquals(teamId),
			ConfigurationArchiveSpecification.NameEquals(name)
		];

		var specification = new CompositeSpecification<ConfigurationArchive>(PredicateOperator.AndAlso, specs);

		var predicate = specification.Satisfy();

		return GetAsync(predicate, query => query.Include(t => t.Configuration), cancellationToken);
	}
}