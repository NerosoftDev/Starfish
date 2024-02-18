using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

internal interface IConfigurationDisableUseCase : INonOutputUseCase<ConfigurationDisableInput>;

internal record ConfigurationDisableInput(string Id) : IUseCaseInput;

internal class ConfigurationDisableUseCase : IConfigurationDisableUseCase
{
	private readonly IBus _bus;

	public ConfigurationDisableUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(ConfigurationDisableInput input, CancellationToken cancellationToken = default)
	{
		var command = new ConfigurationDisableCommand(input.Id);
		return _bus.SendAsync(command, cancellationToken);
	}
}