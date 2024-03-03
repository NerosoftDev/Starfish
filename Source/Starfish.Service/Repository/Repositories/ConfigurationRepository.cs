using System.Linq.Expressions;
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

	public Task<List<ConfigurationItem>> GetItemListAsync(string id, string key, int skip, int count, CancellationToken cancellationToken = default)
	{
		var query = Context.Set<ConfigurationItem>()
		                   .AsQueryable()
		                   .AsNoTracking();
		var expressions = new List<Expression<Func<ConfigurationItem, bool>>>
		{
			t => t.ConfigurationId == id
		};

		if (!string.IsNullOrWhiteSpace(key))
		{
			expressions.Add(t => t.Key.Contains(key));
		}

		var predicate = expressions.Compose();

		return query.Where(predicate)
		            .Skip(skip)
		            .Take(count)
		            .ToListAsync(cancellationToken);
	}

	public Task<int> GetItemCountAsync(string id, string key, Func<IQueryable<ConfigurationItem>, IQueryable<ConfigurationItem>> action, CancellationToken cancellationToken = default)
	{
		var query = Context.Set<ConfigurationItem>()
		                   .AsQueryable();

		if (action != null)
		{
			query = action(query);
		}

		var expressions = new List<Expression<Func<ConfigurationItem, bool>>>
		{
			t => t.Id > 0
		};

		if (!string.IsNullOrWhiteSpace(id))
		{
			expressions.Add(t => t.ConfigurationId == id);
		}

		if (!string.IsNullOrWhiteSpace(key))
		{
			expressions.Add(t => t.Key.Contains(key));
		}

		var predicate = expressions.Compose();

		return query.Where(predicate).CountAsync(cancellationToken);
	}
}