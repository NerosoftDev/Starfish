using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 重命名配置节点用例接口
/// </summary>
public interface ISettingNodeUpdateNameUseCase : IUseCase<SettingNodeUpdateNameInput>;

/// <summary>
/// 重命名配置节点输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
public record SettingNodeUpdateNameInput(long Id, string Name) : IUseCaseInput;

/// <summary>
/// 重命名配置节点用例
/// </summary>
public class SettingNodeUpdateNameUseCase : ISettingNodeUpdateNameUseCase
{
	private readonly IBus _bus;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	public SettingNodeUpdateNameUseCase(IBus bus)
	{
		_bus = bus;
	}

	/// <inheritdoc />
	public Task ExecuteAsync(SettingNodeUpdateNameInput input, CancellationToken cancellationToken = default)
	{
		var command = new SettingNodeUpdateCommand(input.Id, "name", input.Name);
		return _bus.SendAsync(command, cancellationToken);
	}
}