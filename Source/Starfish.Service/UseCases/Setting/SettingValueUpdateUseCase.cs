using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

public interface ISettingValueUpdateUseCase : INonOutputUseCase<SettingValueUpdateInput>;

public record SettingValueUpdateInput(long AppId, string Environment, string Key, string Value) : IUseCaseInput;

public class SettingValueUpdateUseCase : ISettingValueUpdateUseCase
{
	private readonly IBus _bus;

	public SettingValueUpdateUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(SettingValueUpdateInput input, CancellationToken cancellationToken = default)
	{
		var command = new SettingValueUpdateCommand(input.AppId, input.Environment, input.Key, input.Value);
		return _bus.SendAsync(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
		           }, cancellationToken);
	}
}