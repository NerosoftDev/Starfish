using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

public class ConfigurationRepository : BaseRepository<DataContext, Configuration, long>, IConfigurationRepository
{
	public ConfigurationRepository(IContextProvider provider)
		: base(provider)
	{
	}

	public Task<bool> ExistsAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		ISpecification<Configuration>[] specifications =
		[
			ConfigurationSpecification.AppIdEquals(appId),
			ConfigurationSpecification.EnvironmentEquals(environment)
		];

		var predicate = new CompositeSpecification<Configuration>(PredicateOperator.AndAlso, specifications).Satisfy();
		return AnyAsync(predicate, null, cancellationToken);
	}

	public Task<Configuration> GetAsync(long appId, string environment, bool tracking, string[] properties, CancellationToken cancellationToken = default)
	{
		ISpecification<Configuration>[] specifications =
		[
			ConfigurationSpecification.AppIdEquals(appId),
			ConfigurationSpecification.EnvironmentEquals(environment)
		];

		var predicate = new CompositeSpecification<Configuration>(PredicateOperator.AndAlso, specifications).Satisfy();

		return GetAsync(predicate, tracking, properties, cancellationToken);
	}

	public Task<List<ConfigurationItem>> GetItemListAsync(long id, string environment, int skip, int count, CancellationToken cancellationToken = default)
	{
		ISpecification<ConfigurationItem>[] specifications =
		[
			ConfigurationSpecification.ConfigurationAppIdEquals(id),
			ConfigurationSpecification.ConfigurationAppEnvironmentEquals(environment)
		];

		var predicate = new CompositeSpecification<ConfigurationItem>(PredicateOperator.AndAlso, specifications).Satisfy();

		var query = Context.Set<ConfigurationItem>()
		                   .AsQueryable()
		                   .Include(t => t.Configuration);
		return query.Where(predicate)
		            .Skip(skip)
		            .Take(count)
		            .ToListAsync(cancellationToken);
	}

	public Task<int> GetItemCountAsync(long id, string environment, CancellationToken cancellationToken = default)
	{
		ISpecification<ConfigurationItem>[] specifications =
		[
			ConfigurationSpecification.ConfigurationAppIdEquals(id),
			ConfigurationSpecification.ConfigurationAppEnvironmentEquals(environment)
		];

		var predicate = new CompositeSpecification<ConfigurationItem>(PredicateOperator.AndAlso, specifications).Satisfy();

		var query = Context.Set<ConfigurationItem>()
		                   .AsQueryable()
		                   .Include(t => t.Configuration);
		return query.Where(predicate).CountAsync(cancellationToken);
	}
}