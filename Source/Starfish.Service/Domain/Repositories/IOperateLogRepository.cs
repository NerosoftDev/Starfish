using System.Linq.Expressions;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 操作日志仓储
/// </summary>
public interface IOperateLogRepository : IRepository<OperateLog, long>
{
	/// <summary>
	/// 搜索操作日志
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="collator"></param>
	/// <param name="page"></param>
	/// <param name="size"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<OperateLog>> FetchAsync(Expression<Func<OperateLog, bool>> predicate, Func<IQueryable<OperateLog>, IOrderedQueryable<OperateLog>> collator, int page, int size, CancellationToken cancellationToken = default);
}