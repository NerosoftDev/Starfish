using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public interface IConfigurationArchiveRepository : IBaseRepository<DataContext, ConfigurationArchive, string>
{
	Task<ConfigurationArchive> GetAsync(string teamId, string name, CancellationToken cancellationToken = default);
}