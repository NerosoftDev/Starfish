using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public interface ISettingRepository : IBaseRepository<DataContext, Setting, long>
{
	/// <summary>
	/// 检查配置是否存在
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> ExistsAsync(long appId, string environment, CancellationToken cancellationToken = default);

	Task<Setting> GetAsync(long appId, string environment, bool tracking, string[] properties, CancellationToken cancellationToken = default);
	
	Task<List<SettingItem>> GetItemListAsync(long id, string environment, int page, int size, CancellationToken cancellationToken = default);

	Task<int> GetItemCountAsync(long id, string environment, CancellationToken cancellationToken = default);
}