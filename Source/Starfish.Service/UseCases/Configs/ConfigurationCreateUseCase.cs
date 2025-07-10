using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IConfigurationCreateUseCase : IUseCase<ConfigurationCreateInput, long>;

internal record ConfigurationCreateInput(long TeamId, ConfigurationEditDto Data) : IUseCaseInput;

internal class ConfigurationCreateUseCase : IConfigurationCreateUseCase
{
	private readonly IBus _bus;

	public ConfigurationCreateUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task<long> ExecuteAsync(ConfigurationCreateInput input, CancellationToken cancellationToken = default)
	{
		var command = new ConfigurationCreateCommand(input.TeamId);

		command = TypeAdapter.ProjectedAs(input.Data, command);

		return _bus.SendAsync<ConfigurationCreateCommand, long>(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
			           return task.Result;
		           }, cancellationToken);
	}
}