using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Domain;

public interface ISettingArchiveRepository : IRepository<SettingArchive, long>
{
	Task<SettingArchive> GetAsync(string appCode, string environment, CancellationToken cancellationToken = default);
}