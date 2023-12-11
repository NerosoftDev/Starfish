using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 应用详情查询用例接口
/// </summary>
public interface IAppInfoDetailUseCase : IUseCase<AppInfoDetailInput, AppInfoDetailOutput>;

/// <summary>
/// 应用详情查询用例输出
/// </summary>
/// <param name="Result"></param>
public record AppInfoDetailOutput(AppInfoDetailDto Result) : IUseCaseOutput;

/// <summary>
/// 应用详情查询用例输入
/// </summary>
/// <param name="Id"></param>
public record AppInfoDetailInput(long Id) : IUseCaseInput;

/// <summary>
/// 应用详情查询用例
/// </summary>
public class AppInfoDetailUseCase : IAppInfoDetailUseCase
{
	private readonly IAppInfoRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public AppInfoDetailUseCase(IAppInfoRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task<AppInfoDetailOutput> ExecuteAsync(AppInfoDetailInput input, CancellationToken cancellationToken = default)
	{
		var predicate = AppInfoSpecification.IdEquals(input.Id).Satisfy();
		return _repository.GetAsync(predicate, cancellationToken)
		                  .ContinueWith(task => new AppInfoDetailOutput(task.Result.ProjectedAs<AppInfoDetailDto>()), cancellationToken);
	}
}