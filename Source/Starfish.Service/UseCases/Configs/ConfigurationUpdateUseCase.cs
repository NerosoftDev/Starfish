using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IConfigurationUpdateUseCase : INonOutputUseCase<ConfigurationUpdateInput>;

internal record ConfigurationUpdateInput(string Id, ConfigurationEditDto Data);

internal class ConfigurationUpdateUseCase : IConfigurationUpdateUseCase
{
	private readonly IBus _bus;

	public ConfigurationUpdateUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(ConfigurationUpdateInput input, CancellationToken cancellationToken = default)
	{
		var command = new ConfigurationUpdateCommand(input.Id);

		command = TypeAdapter.ProjectedAs(input.Data, command);

		return _bus.SendAsync(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
		           }, cancellationToken);
	}
}