using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 日志数量查询用例接口
/// </summary>
public interface ILogsCountUseCase : IUseCase<LogsCountUseCaseInput, LogsCountUseCaseOutput>;

/// <summary>
/// 日志数量查询用例输入
/// </summary>
/// <param name="Criteria"></param>
public record LogsCountUseCaseInput(OperateLogCriteria Criteria) : IUseCaseInput;

/// <summary>
/// 日志数量查询用例输出
/// </summary>
/// <param name="Count"></param>
public record LogsCountUseCaseOutput(int Count) : IUseCaseOutput;

/// <summary>
/// 日志数量查询用例
/// </summary>
public class LogsCountUseCase : ILogsCountUseCase
{
	private readonly IOperateLogRepository _repository;
	private readonly UserPrincipal _user;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="user"></param>
	public LogsCountUseCase(IOperateLogRepository repository, UserPrincipal user)
	{
		_repository = repository;
		_user = user;
	}

	/// <inheritdoc />
	public async Task<LogsCountUseCaseOutput> ExecuteAsync(LogsCountUseCaseInput input, CancellationToken cancellationToken = default)
	{
		var specification = input.Criteria.GetSpecification();

		if (!_user.IsInRole("SA"))
		{
			specification &= OperateLogSpecification.UserNameEquals(_user.Username);
		}

		var predicate = specification.Satisfy();

		var result = await _repository.CountAsync(predicate, cancellationToken);
		return new LogsCountUseCaseOutput(result);
	}
}