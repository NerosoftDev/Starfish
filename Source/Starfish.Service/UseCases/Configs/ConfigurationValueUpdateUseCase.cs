using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

public interface IConfigurationValueUpdateUseCase : INonOutputUseCase<ConfigurationValueUpdateInput>;

public record ConfigurationValueUpdateInput(string Id, string Key, string Value) : IUseCaseInput;

public class ConfigurationValueUpdateUseCase : IConfigurationValueUpdateUseCase
{
	private readonly IBus _bus;

	public ConfigurationValueUpdateUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(ConfigurationValueUpdateInput input, CancellationToken cancellationToken = default)
	{
		var command = new ConfigurationValueUpdateCommand(input.Id, input.Key, input.Value);
		return _bus.SendAsync(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
		           }, cancellationToken);
	}
}