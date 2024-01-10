using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 日志应用服务接口
/// </summary>
public interface ILogsApplicationService : IApplicationService
{
	/// <summary>
	/// 搜索日志
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="page"></param>
	/// <param name="size"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<OperateLogDto>> QueryAsync(OperateLogCriteria criteria, int page, int size, CancellationToken cancellationToken = default);

	/// <summary>
	/// 查询日志数量
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<int> CountAsync(OperateLogCriteria criteria, CancellationToken cancellationToken = default);
}