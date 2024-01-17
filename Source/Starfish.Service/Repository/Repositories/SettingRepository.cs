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

	public Task<Setting> GetAsync(long appId, string environment, bool tracking, string[] properties, CancellationToken cancellationToken = default)
	{
		ISpecification<Setting>[] specifications =
		[
			SettingSpecification.AppIdEquals(appId),
			SettingSpecification.EnvironmentEquals(environment)
		];

		var predicate = new CompositeSpecification<Setting>(PredicateOperator.AndAlso, specifications).Satisfy();

		return GetAsync(predicate, tracking, properties, cancellationToken);
	}
	
	public Task<List<SettingItem>> GetItemListAsync(long id, string environment, int page, int size, CancellationToken cancellationToken = default)
	{
		ISpecification<SettingItem>[] specifications =
		[
			SettingSpecification.SettingAppIdEquals(id),
			SettingSpecification.SettingAppEnvironmentEquals(environment)
		];

		var predicate = new CompositeSpecification<SettingItem>(PredicateOperator.AndAlso, specifications).Satisfy();

		var query = Context.Set<SettingItem>()
		                   .AsQueryable()
		                   .Include(t => t.Setting);
		return query.Where(predicate)
		            .Paginate(page, size)
		            .ToListAsync(cancellationToken);
	}

	public Task<int> GetItemCountAsync(long id, string environment, CancellationToken cancellationToken = default)
	{
		ISpecification<SettingItem>[] specifications =
		[
			SettingSpecification.SettingAppIdEquals(id),
			SettingSpecification.SettingAppEnvironmentEquals(environment)
		];

		var predicate = new CompositeSpecification<SettingItem>(PredicateOperator.AndAlso, specifications).Satisfy();

		var query = Context.Set<SettingItem>()
		                   .AsQueryable()
		                   .Include(t => t.Setting);
		return query.Where(predicate).CountAsync(cancellationToken);
	}
}