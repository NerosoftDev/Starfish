using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

public class SettingArchiveRepository : BaseRepository<DataContext, SettingArchive, long>, ISettingArchiveRepository
{
	public SettingArchiveRepository(IContextProvider provider)
		: base(provider)
	{
	}

	public Task<SettingArchive> GetAsync(string appCode, string environment, CancellationToken cancellationToken = default)
	{
		ISpecification<SettingArchive>[] specs =
		[
			SettingArchiveSpecification.AppCodeEquals(appCode),
			SettingArchiveSpecification.EnvironmentEquals(environment)
		];

		var specification = new CompositeSpecification<SettingArchive>(PredicateOperator.AndAlso, specs);

		var predicate = specification.Satisfy();

		return GetAsync(predicate, true, cancellationToken);
	}
}