using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public interface ISettingArchiveRepository : IBaseRepository<DataContext, SettingArchive, long>
{
	Task<SettingArchive> GetAsync(string appCode, string environment, CancellationToken cancellationToken = default);
}