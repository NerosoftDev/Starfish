using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 获取配置节点详情用例接口
/// </summary>
public interface ISettingNodeGetDetailUseCase : IUseCase<SettingNodeGetDetailInput, SettingNodeGetDetailOutput>;

/// <summary>
/// 获取配置节点详情用例输出
/// </summary>
/// <param name="Result"></param>
public record SettingNodeGetDetailOutput(SettingNodeDetailDto Result) : IUseCaseOutput;

/// <summary>
/// 获取配置节点详情用例输入
/// </summary>
/// <param name="Id"></param>
public record SettingNodeGetDetailInput(long Id) : IUseCaseInput;

/// <summary>
/// 获取配置节点详情用例
/// </summary>
public class SettingNodeGetDetailUseCase : ISettingNodeGetDetailUseCase
{
	private readonly ISettingNodeRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public SettingNodeGetDetailUseCase(ISettingNodeRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task<SettingNodeGetDetailOutput> ExecuteAsync(SettingNodeGetDetailInput input, CancellationToken cancellationToken = default)
	{
		return _repository.GetAsync(input.Id, false, Array.Empty<string>(), cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = TypeAdapter.ProjectedAs<SettingNodeDetailDto>(task.Result);
			                  return new SettingNodeGetDetailOutput(result);
		                  }, cancellationToken);
	}
}