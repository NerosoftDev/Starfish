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
public record GetConfigurationDetailOutput(ConfigurationDto Result) : IUseCaseOutput;

/// <summary>
/// 获取配置节点详情用例输入
/// </summary>
/// <param name="Id"></param>
public record GetConfigurationDetailInput(string Id) : IUseCaseInput;

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
		return _repository.GetAsync(input.Id, false, [], cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = TypeAdapter.ProjectedAs<ConfigurationDto>(task.Result);
			                  return new GetConfigurationDetailOutput(result);
		                  }, cancellationToken);
	}
}