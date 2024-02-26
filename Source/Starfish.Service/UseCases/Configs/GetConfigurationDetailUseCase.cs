using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 获取配置节点详情用例接口
/// </summary>
internal interface IGetConfigurationDetailUseCase : IUseCase<GetConfigurationDetailInput, GetConfigurationDetailOutput>;

/// <summary>
/// 获取配置节点详情用例输出
/// </summary>
/// <param name="Result"></param>
internal record GetConfigurationDetailOutput(ConfigurationDto Result) : IUseCaseOutput;

/// <summary>
/// 获取配置节点详情用例输入
/// </summary>
/// <param name="Id"></param>
internal record GetConfigurationDetailInput(string Id) : IUseCaseInput;

/// <summary>
/// 获取配置节点详情用例
/// </summary>
internal class GetConfigurationDetailUseCase : IGetConfigurationDetailUseCase
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
		return _repository.GetAsync(input.Id, false, [], cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = TypeAdapter.ProjectedAs<ConfigurationDto>(task.Result);
			                  return new GetConfigurationDetailOutput(result);
		                  }, cancellationToken);
	}
}