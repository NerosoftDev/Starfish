using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 配置节点更新用例接口
/// </summary>
public interface ISettingNodeUpdateValueUseCase : IUseCase<SettingNodeUpdateValueInput>;

/// <summary>
/// 配置节点更新输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Value"></param>
public record SettingNodeUpdateValueInput(long Id, string Value) : IUseCaseInput;

/// <summary>
/// 配置节点更新用例
/// </summary>
public class SettingNodeUpdateValueUseCase : ISettingNodeUpdateValueUseCase
{
	private readonly IBus _bus;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	public SettingNodeUpdateValueUseCase(IBus bus)
	{
		_bus = bus;
	}

	/// <inheritdoc />
	public Task ExecuteAsync(SettingNodeUpdateValueInput valueInput, CancellationToken cancellationToken = default)
	{
		var command = new SettingNodeUpdateCommand(valueInput.Id, "Value", valueInput.Value);
		return _bus.SendAsync(command, cancellationToken);
	}
}