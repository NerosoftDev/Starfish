using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

internal interface IConfigurationEnableUseCase : INonOutputUseCase<ConfigurationEnableInput>;

internal record ConfigurationEnableInput(string Id) : IUseCaseInput;

internal class ConfigurationEnableUseCase : IConfigurationEnableUseCase
{
	private readonly IBus _bus;

	public ConfigurationEnableUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(ConfigurationEnableInput input, CancellationToken cancellationToken = default)
	{
		var command = new ConfigurationEnableCommand(input.Id);
		return _bus.SendAsync(command, cancellationToken);
	}
}