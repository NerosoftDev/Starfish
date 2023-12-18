using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 新增根节点用例接口
/// </summary>
public interface ISettingRootNodeCreateUseCase : IUseCase<SettingRootNodeCreationInput, SettingRootNodeCreationOutput>;

/// <summary>
/// 新增根节点输出
/// </summary>
/// <param name="Id"></param>
public record SettingRootNodeCreationOutput(long Id) : IUseCaseOutput;

/// <summary>
/// 新增根节点输入
/// </summary>
/// <param name="AppId"></param>
/// <param name="Environment"></param>
public record SettingRootNodeCreationInput(long AppId, string Environment) : IUseCaseInput;

/// <summary>
/// 新增根节点用例
/// </summary>
public class SettingRootNodeCreateUseCase : ISettingRootNodeCreateUseCase
{
	private readonly IBus _bus;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	public SettingRootNodeCreateUseCase(IBus bus)
	{
		_bus = bus;
	}

	/// <inheritdoc />
	public Task<SettingRootNodeCreationOutput> ExecuteAsync(SettingRootNodeCreationInput input, CancellationToken cancellationToken = default)
	{
		var command = new SettingRootNodeCreateCommand
		{
			AppId = input.AppId,
			Environment = input.Environment
		};
		return _bus.SendAsync<SettingRootNodeCreateCommand, long>(command, cancellationToken)
		           .ContinueWith(t => new SettingRootNodeCreationOutput(t.Result), cancellationToken);
	}
}