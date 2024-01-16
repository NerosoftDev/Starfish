using System.Linq.Expressions;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Domain;

public interface ISettingRepository : IRepository<Setting, long>
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

	Task<Setting> GetAsync(Expression<Func<Setting, bool>> predicate, bool tracking, string[] properties, CancellationToken cancellationToken = default);

	/// <summary>
	/// 查询符合条件的配置列表
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="action"></param>
	/// <param name="page"></param>
	/// <param name="size"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<Setting>> FindAsync(Expression<Func<Setting, bool>> predicate, Func<IQueryable<Setting>, IQueryable<Setting>> action, int page, int size, CancellationToken cancellationToken = default);

	Task<List<SettingItem>> GetItemListAsync(long id, string environment, int page, int size, CancellationToken cancellationToken = default);

	Task<int> GetItemCountAsync(long id, string environment, CancellationToken cancellationToken = default);
}