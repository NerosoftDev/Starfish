using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 配置节点更新用例接口
/// </summary>
public interface ISettingNodeUpdateUseCase : IUseCase<SettingNodeUpdateInput>;

/// <summary>
/// 配置节点更新输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Data"></param>
public record SettingNodeUpdateInput(long Id, SettingNodeUpdateDto Data) : IUseCaseInput;

/// <summary>
/// 配置节点更新用例
/// </summary>
public class SettingNodeUpdateUseCase : ISettingNodeUpdateUseCase
{
	private readonly IBus _bus;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	public SettingNodeUpdateUseCase(IBus bus)
	{
		_bus = bus;
	}

	/// <inheritdoc />
	public Task ExecuteAsync(SettingNodeUpdateInput input, CancellationToken cancellationToken = default)
	{
		var command = new SettingNodeUpdateCommand(input.Id, input.Data);
		return _bus.SendAsync(command, cancellationToken);
	}
}