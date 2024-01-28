using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public interface IConfigurationArchiveRepository : IBaseRepository<DataContext, ConfigurationArchive, long>
{
	Task<ConfigurationArchive> GetAsync(long appId, string environment, CancellationToken cancellationToken = default);
}