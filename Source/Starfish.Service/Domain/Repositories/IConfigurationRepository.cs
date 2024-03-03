using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public interface IConfigurationRepository : IBaseRepository<DataContext, Configuration, string>
{
	/// <summary>
	/// 检查配置是否存在
	/// </summary>
	/// <param name="teamId"></param>
	/// <param name="name"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> ExistsAsync(string teamId, string name, CancellationToken cancellationToken = default);

	Task<List<ConfigurationItem>> GetItemListAsync(string id, string key, int skip, int count, CancellationToken cancellationToken = default);

	Task<int> GetItemCountAsync(string id, string key, Func<IQueryable<ConfigurationItem>,IQueryable<ConfigurationItem>> action, CancellationToken cancellationToken = default);
}