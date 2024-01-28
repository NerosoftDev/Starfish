using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 获取配置节点详情用例接口
/// </summary>
public interface IGetConfigurationDetailUseCase : IUseCase<GetConfigurationDetailInput, GetConfigurationDetailOutput>;

/// <summary>
/// 获取配置节点详情用例输出
/// </summary>
/// <param name="Result"></param>
public record GetConfigurationDetailOutput(ConfigurationDetailDto Result) : IUseCaseOutput;

/// <summary>
/// 获取配置节点详情用例输入
/// </summary>
/// <param name="AppId"></param>
public record GetConfigurationDetailInput(long AppId, string Environment) : IUseCaseInput;

/// <summary>
/// 获取配置节点详情用例
/// </summary>
public class GetConfigurationDetailUseCase : IGetConfigurationDetailUseCase
{
	private readonly IConfigurationRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public GetConfigurationDetailUseCase(IConfigurationRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task<GetConfigurationDetailOutput> ExecuteAsync(GetConfigurationDetailInput input, CancellationToken cancellationToken = default)
	{
		ISpecification<Configuration>[] specifications =
		[
			ConfigurationSpecification.AppIdEquals(input.AppId),
			ConfigurationSpecification.EnvironmentEquals(input.Environment)
		];

		var predicate = new CompositeSpecification<Configuration>(PredicateOperator.AndAlso, specifications).Satisfy();

		return _repository.GetAsync(predicate, false, [nameof(Configuration.App)], cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = TypeAdapter.ProjectedAs<ConfigurationDetailDto>(task.Result);
			                  return new GetConfigurationDetailOutput(result);
		                  }, cancellationToken);
	}
}