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
internal interface ILogsQueryUseCase : IUseCase<GenericQueryInput<OperateLogCriteria>, LogsQueryUseCaseOutput>;

/// <summary>
/// 日志搜索用例输出
/// </summary>
/// <param name="Logs"></param>
internal record LogsQueryUseCaseOutput(List<OperateLogDto> Logs) : IUseCaseOutput;

/// <summary>
/// 日志搜索用例
/// </summary>
internal class LogsQueryUseCase : ILogsQueryUseCase
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
	public async Task<LogsQueryUseCaseOutput> ExecuteAsync(GenericQueryInput<OperateLogCriteria> input, CancellationToken cancellationToken = default)
	{
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