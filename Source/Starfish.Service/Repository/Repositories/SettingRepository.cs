using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

public class SettingRepository : BaseRepository<DataContext, Setting, long>, ISettingRepository
{
	public SettingRepository(IContextProvider provider)
		: base(provider)
	{
	}

	public Task<bool> ExistsAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		ISpecification<Setting>[] specifications =
		[
			SettingSpecification.AppIdEquals(appId),
			SettingSpecification.EnvironmentEquals(environment)
		];

		var predicate = new CompositeSpecification<Setting>(PredicateOperator.AndAlso, specifications).Satisfy();
		return ExistsAsync(predicate, cancellationToken);
	}

	public Task<List<Setting>> FindAsync(Expression<Func<Setting, bool>> predicate, Func<IQueryable<Setting>, IQueryable<Setting>> action, int page, int size, CancellationToken cancellationToken = default)
	{
		var query = Context.Set<Setting>().AsQueryable();
		query = query.Where(predicate);
		if (action != null)
		{
			query = action(query);
		}

		return query.Paginate(page, size).ToListAsync(cancellationToken);
	}
}