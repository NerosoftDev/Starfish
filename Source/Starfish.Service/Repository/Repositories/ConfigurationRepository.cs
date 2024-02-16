using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

public class ConfigurationRepository : BaseRepository<DataContext, Configuration, string>, IConfigurationRepository
{
	public ConfigurationRepository(IContextProvider provider)
		: base(provider)
	{
	}

	public Task<bool> ExistsAsync(string teamId, string name, CancellationToken cancellationToken = default)
	{
		ISpecification<Configuration>[] specifications =
		[
			ConfigurationSpecification.TeamIdEquals(teamId),
			ConfigurationSpecification.NameEquals(name)
		];

		var predicate = new CompositeSpecification<Configuration>(PredicateOperator.AndAlso, specifications).Satisfy();
		return AnyAsync(predicate, null, cancellationToken);
	}

	public Task<List<ConfigurationItem>> GetItemListAsync(string id, int skip, int count, CancellationToken cancellationToken = default)
	{
		var query = Context.Set<ConfigurationItem>()
		                   .AsQueryable()
		                   .AsNoTracking();
		return query.Where(t => t.ConfigurationId == id)
		            .Skip(skip)
		            .Take(count)
		            .ToListAsync(cancellationToken);
	}

	public Task<int> GetItemCountAsync(string id, CancellationToken cancellationToken = default)
	{
		var query = Context.Set<ConfigurationItem>()
		                   .AsQueryable();
		return query.Where(t => t.ConfigurationId == id).CountAsync(cancellationToken);
	}
}