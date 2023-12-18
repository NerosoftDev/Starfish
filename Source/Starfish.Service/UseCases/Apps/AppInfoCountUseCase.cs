using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 应用数量查询用例接口
/// </summary>
public interface IAppInfoCountUseCase : IUseCase<AppInfoCountInput, AppInfoCountOutput>;

/// <summary>
/// 应用数量查询用例输出
/// </summary>
/// <param name="Count"></param>
public record AppInfoCountOutput(int Count) : IUseCaseOutput;

/// <summary>
/// 应用数量查询用例输入
/// </summary>
/// <param name="Criteria"></param>
public record AppInfoCountInput(AppInfoCriteria Criteria) : IUseCaseInput;

/// <summary>
/// 应用数量查询用例
/// </summary>
public class AppInfoCountUseCase : IAppInfoCountUseCase
{
	private readonly IAppInfoRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public AppInfoCountUseCase(IAppInfoRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public async Task<AppInfoCountOutput> ExecuteAsync(AppInfoCountInput input, CancellationToken cancellationToken = default)
	{
		var predicate = input.Criteria.GetSpecification().Satisfy();
		var count = await _repository.CountAsync(predicate, cancellationToken);
		return new AppInfoCountOutput(count);
	}
}