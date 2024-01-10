using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Common;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface ISettingUpdateUseCase : INonOutputUseCase<SettingUpdateInput>;

public record SettingUpdateInput(long Id, SettingUpdateDto Data);

public class SettingUpdateUseCase : ISettingUpdateUseCase
{
	private readonly IBus _bus;

	public SettingUpdateUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(SettingUpdateInput input, CancellationToken cancellationToken = default)
	{
		var data = Cryptography.Base64.Decrypt(input.Data.Data);
		var command = new SettingUpdateCommand(input.Id)
		{
			Data = input.Data.Type switch
			{
				"json" => JsonConfigurationFileParser.Parse(data),
				"text" => TextConfigurationFileParser.Parse(data),
				_ => default
			}
		};
		return _bus.SendAsync(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
		           }, cancellationToken);
	}
}