using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 删除配置节点用例接口
/// </summary>
public interface ISettingNodeDeleteUseCase : IUseCase<SettingNodeDeleteInput>;

/// <summary>
/// 删除配置节点输入
/// </summary>
/// <param name="Id"></param>
public record SettingNodeDeleteInput(long Id) : IUseCaseInput;

/// <summary>
/// 删除配置节点用例
/// </summary>
public class SettingNodeDeleteUseCase : ISettingNodeDeleteUseCase
{
	private readonly IBus _bus;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	public SettingNodeDeleteUseCase(IBus bus)
	{
		_bus = bus;
	}

	/// <inheritdoc />
	public Task ExecuteAsync(SettingNodeDeleteInput input, CancellationToken cancellationToken = default)
	{
		var command = new SettingNodeDeleteCommand(input.Id);
		return _bus.SendAsync(command, cancellationToken);
	}
}