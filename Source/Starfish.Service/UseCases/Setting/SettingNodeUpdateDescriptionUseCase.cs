using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 配置节点更新用例接口
/// </summary>
public interface ISettingNodeUpdateDescriptionUseCase : IUseCase<SettingNodeUpdateDescriptionInput>;

/// <summary>
/// 配置节点更新输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Description"></param>
public record SettingNodeUpdateDescriptionInput(long Id, string Description) : IUseCaseInput;

/// <summary>
/// 配置节点更新用例
/// </summary>
public class SettingNodeUpdateDescriptionUseCase : ISettingNodeUpdateDescriptionUseCase
{
	private readonly IBus _bus;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	public SettingNodeUpdateDescriptionUseCase(IBus bus)
	{
		_bus = bus;
	}

	/// <inheritdoc />
	public Task ExecuteAsync(SettingNodeUpdateDescriptionInput valueInput, CancellationToken cancellationToken = default)
	{
		var command = new SettingNodeUpdateCommand(valueInput.Id, "description", valueInput.Description);
		return _bus.SendAsync(command, cancellationToken);
	}
}