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
public interface IGetSettingDetailUseCase : IUseCase<GetSettingDetailInput, GetSettingDetailOutput>;

/// <summary>
/// 获取配置节点详情用例输出
/// </summary>
/// <param name="Result"></param>
public record GetSettingDetailOutput(SettingDetailDto Result) : IUseCaseOutput;

/// <summary>
/// 获取配置节点详情用例输入
/// </summary>
/// <param name="AppId"></param>
public record GetSettingDetailInput(long AppId, string Environment) : IUseCaseInput;

/// <summary>
/// 获取配置节点详情用例
/// </summary>
public class GetSettingDetailUseCase : IGetSettingDetailUseCase
{
	private readonly ISettingRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public GetSettingDetailUseCase(ISettingRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task<GetSettingDetailOutput> ExecuteAsync(GetSettingDetailInput input, CancellationToken cancellationToken = default)
	{
		ISpecification<Setting>[] specifications =
		[
			SettingSpecification.AppIdEquals(input.AppId),
			SettingSpecification.EnvironmentEquals(input.Environment)
		];

		var predicate = new CompositeSpecification<Setting>(PredicateOperator.AndAlso, specifications).Satisfy();

		return _repository.GetAsync(predicate, false, [nameof(Setting.App)], cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = TypeAdapter.ProjectedAs<SettingDetailDto>(task.Result);
			                  return new GetSettingDetailOutput(result);
		                  }, cancellationToken);
	}
}