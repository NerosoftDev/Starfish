using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 获取符合条件的配置数量用例接口
/// </summary>
public interface ISettingNodeCountUseCase : IUseCase<SettingNodeCountInput, SettingNodeCountOutput>;

/// <summary>
/// 获取符合条件的配置数量用例输出
/// </summary>
/// <param name="Result"></param>
public record SettingNodeCountOutput(int Result) : IUseCaseOutput;

/// <summary>
/// 获取符合条件的配置数量用例输入
/// </summary>
/// <param name="Criteria"></param>
public record SettingNodeCountInput(SettingNodeCriteria Criteria) : IUseCaseInput;

/// <summary>
/// 获取符合条件的配置数量用例
/// </summary>
public class SettingNodeCountUseCase : ISettingNodeCountUseCase
{
	private readonly ISettingNodeRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public SettingNodeCountUseCase(ISettingNodeRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task<SettingNodeCountOutput> ExecuteAsync(SettingNodeCountInput input, CancellationToken cancellationToken = default)
	{
		var specification = input.Criteria.GetSpecification();
		var predicate = specification.Satisfy();
		return _repository.CountAsync(predicate, cancellationToken)
		                  .ContinueWith(t => new SettingNodeCountOutput(t.Result), cancellationToken);
	}
}