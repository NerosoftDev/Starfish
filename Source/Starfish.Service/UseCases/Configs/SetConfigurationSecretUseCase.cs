using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

internal interface ISetConfigurationSecretUseCase : INonOutputUseCase<SetConfigurationSecretInput>;

internal record SetConfigurationSecretInput(string Id, string Secret) : IUseCaseInput;

internal class SetConfigurationSecretUseCase : ISetConfigurationSecretUseCase
{
	private readonly IBus _bus;

	public SetConfigurationSecretUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(SetConfigurationSecretInput input, CancellationToken cancellationToken = default)
	{
		var command = new ConfigurationSetSecretCommand(input.Id, input.Secret);
		return _bus.SendAsync(command, cancellationToken);
	}
}