using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 获取配置节点详情用例接口
/// </summary>
public interface ISettingGetDetailUseCase : IUseCase<SettingGetDetailInput, SettingGetDetailOutput>;

/// <summary>
/// 获取配置节点详情用例输出
/// </summary>
/// <param name="Result"></param>
public record SettingGetDetailOutput(SettingDetailDto Result) : IUseCaseOutput;

/// <summary>
/// 获取配置节点详情用例输入
/// </summary>
/// <param name="Id"></param>
public record SettingGetDetailInput(long Id) : IUseCaseInput;

/// <summary>
/// 获取配置节点详情用例
/// </summary>
public class SettingGetDetailUseCase : ISettingGetDetailUseCase
{
	private readonly ISettingRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public SettingGetDetailUseCase(ISettingRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task<SettingGetDetailOutput> ExecuteAsync(SettingGetDetailInput input, CancellationToken cancellationToken = default)
	{
		return _repository.GetAsync(input.Id, false, [nameof(Setting.App)], cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = TypeAdapter.ProjectedAs<SettingDetailDto>(task.Result);
			                  return new SettingGetDetailOutput(result);
		                  }, cancellationToken);
	}
}