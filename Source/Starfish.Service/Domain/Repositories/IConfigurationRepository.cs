using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public interface IConfigurationRepository : IBaseRepository<DataContext, Configuration, long>
{
	/// <summary>
	/// 检查配置是否存在
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> ExistsAsync(string appId, string environment, CancellationToken cancellationToken = default);

	Task<Configuration> GetAsync(string appId, string environment, bool tracking, string[] properties, CancellationToken cancellationToken = default);
	
	Task<List<ConfigurationItem>> GetItemListAsync(string appId, string environment, int skip, int count, CancellationToken cancellationToken = default);

	Task<int> GetItemCountAsync(string appId, string environment, CancellationToken cancellationToken = default);
}