using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 删除配置节点用例接口
/// </summary>
public interface IConfigurationDeleteUseCase : INonOutputUseCase<ConfigurationDeleteInput>;

/// <summary>
/// 删除配置节点输入
/// </summary>
/// <param name="Id"></param>
public record ConfigurationDeleteInput(string Id) : IUseCaseInput;

/// <summary>
/// 删除配置节点用例
/// </summary>
public class ConfigurationDeleteUseCase : IConfigurationDeleteUseCase
{
	private readonly IBus _bus;

	public ConfigurationDeleteUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(ConfigurationDeleteInput input, CancellationToken cancellationToken = default)
	{
		var command = new ConfigurationDeleteCommand(input.Id);
		return _bus.SendAsync(command, cancellationToken);
	}
}