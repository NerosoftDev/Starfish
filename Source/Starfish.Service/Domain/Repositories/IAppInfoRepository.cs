using System.Linq.Expressions;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用信息仓储接口
/// </summary>
public interface IAppInfoRepository : IRepository<AppInfo, long>
{
	/// <summary>
	/// 通过Code获取应用信息
	/// </summary>
	/// <param name="code"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<AppInfo> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

	/// <summary>
	/// 查询符合条件的应用信息数据
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="collator"></param>
	/// <param name="page"></param>
	/// <param name="size"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<AppInfo>> FetchAsync(Expression<Func<AppInfo, bool>> predicate, Func<IQueryable<AppInfo>, IOrderedQueryable<AppInfo>> collator, int page, int size, CancellationToken cancellationToken = default);
}