using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 获取符合条件的配置数量用例接口
/// </summary>
internal interface IGetConfigurationItemCountUseCase : IUseCase<GetConfigurationItemCountInput, GetConfigurationItemCountOutput>;

/// <summary>
/// 获取符合条件的配置数量用例输出
/// </summary>
/// <param name="Result"></param>
internal record GetConfigurationItemCountOutput(int Result) : IUseCaseOutput;

/// <summary>
/// 获取符合条件的配置数量用例输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Key"></param>
internal record GetConfigurationItemCountInput(string Id, string Key) : IUseCaseInput;

/// <summary>
/// 获取符合条件的配置数量用例
/// </summary>
internal class GetConfigurationItemCountUseCase : IGetConfigurationItemCountUseCase
{
	private readonly IConfigurationRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public GetConfigurationItemCountUseCase(IConfigurationRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task<GetConfigurationItemCountOutput> ExecuteAsync(GetConfigurationItemCountInput input, CancellationToken cancellationToken = default)
	{
		return _repository.GetItemCountAsync(input.Id, input.Key, cancellationToken)
		                  .ContinueWith(t => new GetConfigurationItemCountOutput(t.Result), cancellationToken);
	}
}