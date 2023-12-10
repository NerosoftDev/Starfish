using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 日志搜索用例
/// </summary>
public interface ILogsSearchUseCase : IUseCase<LogsSearchUseCaseInput, LogsSearchUseCaseOutput>
{
}

/// <summary>
/// 日志搜索用例输入
/// </summary>
/// <param name="Criteria"></param>
/// <param name="Page"></param>
/// <param name="Size"></param>
public record LogsSearchUseCaseInput(OperateLogCriteria Criteria, int Page, int Size) : IUseCaseInput;

/// <summary>
/// 日志搜索用例输出
/// </summary>
/// <param name="Logs"></param>
public record LogsSearchUseCaseOutput(List<OperateLogDatamodel> Logs) : IUseCaseOutput;

/// <summary>
/// 日志搜索用例
/// </summary>
public class LogsSearchUseCase : ILogsSearchUseCase
{
	private readonly IOperateLogRepository _repository;
	private readonly UserPrincipal _user;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="user"></param>
	public LogsSearchUseCase(IOperateLogRepository repository, UserPrincipal user)
	{
		_repository = repository;
		_user = user;
	}

	/// <inheritdoc />
	public async Task<LogsSearchUseCaseOutput> ExecuteAsync(LogsSearchUseCaseInput input, CancellationToken cancellationToken = new CancellationToken())
	{
		var specification = input.Criteria.GetSpecification();

		if (!_user.IsInRole("SU"))
		{
			specification &= (Specification<OperateLog>)OperateLogSpecification.UserNameEquals(_user.Username);
		}

		var predicate = specification.Satisfy();

		var entities = await _repository.FetchAsync(predicate, Collator, input.Page, input.Size, cancellationToken);
		var items = entities.ProjectedAsCollection<OperateLogDatamodel>();
		return new LogsSearchUseCaseOutput(items);

		IOrderedQueryable<OperateLog> Collator(IQueryable<OperateLog> query) => query.OrderByDescending(t => t.OperateTime);
	}

	public IUseCasePresenter<LogsSearchUseCaseOutput> Presenter { get; }
}