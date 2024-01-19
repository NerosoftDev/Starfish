using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;
using Nerosoft.Starfish.UseCases;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 日志应用服务
/// </summary>
public class LogsApplicationService : BaseApplicationService, ILogsApplicationService
{
	/// <inheritdoc />
	public Task<List<OperateLogDto>> QueryAsync(OperateLogCriteria criteria, int skip, int count, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ILogsQueryUseCase>();
		return useCase.ExecuteAsync(new LogsQueryUseCaseInput(criteria, skip, count), cancellationToken)
					  .ContinueWith(task => task.Result.Logs, cancellationToken);
	}

	/// <inheritdoc />
	public Task<int> CountAsync(OperateLogCriteria criteria, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ILogsCountUseCase>();
		return useCase.ExecuteAsync(new LogsCountUseCaseInput(criteria), cancellationToken)
					  .ContinueWith(task => task.Result.Count, cancellationToken);
	}
}