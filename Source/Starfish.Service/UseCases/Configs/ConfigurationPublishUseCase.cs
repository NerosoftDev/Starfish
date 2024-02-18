using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 配置节点发布用例接口
/// </summary>
public interface IConfigurationPublishUseCase : INonOutputUseCase<ConfigurationPublishInput>;

/// <summary>
/// 配置节点发布输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Data"></param>
public record ConfigurationPublishInput(string Id, ConfigurationPublishRequestDto Data) : IUseCaseInput;

/// <summary>
/// 配置节点发布用例
/// </summary>
public class ConfigurationPublishUseCase : IConfigurationPublishUseCase
{
	private readonly IBus _bus;

	public ConfigurationPublishUseCase(IBus bus)
	{
		_bus = bus;
	}

	public async Task ExecuteAsync(ConfigurationPublishInput input, CancellationToken cancellationToken = default)
	{
		var command = new ConfigurationPublishCommand(input.Id, input.Data.Version, input.Data.Comment);
		await _bus.SendAsync(command, cancellationToken);
	}
}