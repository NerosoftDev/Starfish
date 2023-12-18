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
	public Task<List<OperateLogDto>> SearchAsync(OperateLogCriteria criteria, int page, int size, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ILogsSearchUseCase>();
		return useCase.ExecuteAsync(new LogsSearchUseCaseInput(criteria, page, size), cancellationToken)
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