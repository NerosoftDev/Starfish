using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 获取符合条件的配置数量用例接口
/// </summary>
public interface IGetSettingItemCountUseCase : IUseCase<GetSettingItemCountInput, GetSettingItemCountOutput>;

/// <summary>
/// 获取符合条件的配置数量用例输出
/// </summary>
/// <param name="Result"></param>
public record GetSettingItemCountOutput(int Result) : IUseCaseOutput;

/// <summary>
/// 获取符合条件的配置数量用例输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Environment"></param>
public record GetSettingItemCountInput(long Id, string Environment) : IUseCaseInput;

/// <summary>
/// 获取符合条件的配置数量用例
/// </summary>
public class GetSettingItemCountUseCase : IGetSettingItemCountUseCase
{
	private readonly ISettingRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public GetSettingItemCountUseCase(ISettingRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task<GetSettingItemCountOutput> ExecuteAsync(GetSettingItemCountInput input, CancellationToken cancellationToken = default)
	{
		return _repository.GetItemCountAsync(input.Id, input.Environment, cancellationToken)
		                  .ContinueWith(t => new GetSettingItemCountOutput(t.Result), cancellationToken);
	}
}