using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 日志搜索用例
/// </summary>
public interface ILogsQueryUseCase : IUseCase<LogsQueryUseCaseInput, LogsQueryUseCaseOutput>
{
}

/// <summary>
/// 日志搜索用例输入
/// </summary>
/// <param name="Criteria"></param>
/// <param name="Skip"></param>
/// <param name="Count"></param>
public record LogsQueryUseCaseInput(OperateLogCriteria Criteria, int Skip, int Count) : IUseCaseInput;

/// <summary>
/// 日志搜索用例输出
/// </summary>
/// <param name="Logs"></param>
public record LogsQueryUseCaseOutput(List<OperateLogDto> Logs) : IUseCaseOutput;

/// <summary>
/// 日志搜索用例
/// </summary>
public class LogsQueryUseCase : ILogsQueryUseCase
{
	private readonly IOperateLogRepository _repository;
	private readonly UserPrincipal _identity;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="identity"></param>
	public LogsQueryUseCase(IOperateLogRepository repository, UserPrincipal identity)
	{
		_repository = repository;
		_identity = identity;
	}

	/// <inheritdoc />
	public async Task<LogsQueryUseCaseOutput> ExecuteAsync(LogsQueryUseCaseInput input, CancellationToken cancellationToken = default)
	{
		if (input.Skip < 0)
		{
			throw new BadRequestException(Resources.IDS_ERROR_PARAM_SKIP_CANNOT_BE_NEGATIVE);
		}

		if (input.Count <= 0)
		{
			throw new BadRequestException(Resources.IDS_ERROR_PARAM_COUNT_MUST_GREATER_THAN_0);
		}

		if (!_identity.IsAuthenticated)
		{
			throw new AuthenticationException();
		}

		var specification = input.Criteria.GetSpecification();

		if (!_identity.IsInRole("SA"))
		{
			specification &= OperateLogSpecification.UserNameEquals(_identity.Username);
		}

		var predicate = specification.Satisfy();

		var entities = await _repository.FindAsync(predicate, Collator, input.Skip, input.Count, cancellationToken);
		var items = entities.ProjectedAsCollection<OperateLogDto>();
		return new LogsQueryUseCaseOutput(items);

		static IOrderedQueryable<OperateLog> Collator(IQueryable<OperateLog> query) => query.OrderByDescending(t => t.OperateTime);
	}
}