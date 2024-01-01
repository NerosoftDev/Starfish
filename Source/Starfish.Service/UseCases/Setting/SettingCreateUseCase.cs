using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Common;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface ISettingCreateUseCase : IUseCase<SettingCreateDto, long>;

public class SettingCreateUseCase : ISettingCreateUseCase
{
	private readonly IBus _bus;

	public SettingCreateUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task<long> ExecuteAsync(SettingCreateDto input, CancellationToken cancellationToken = default)
	{
		var data = Cryptography.Base64.Decrypt(input.Data);
		var command = TypeAdapter.ProjectedAs<SettingCreateCommand>(input);
		command.Data = input.Type switch
		{
			"json" => JsonConfigurationFileParser.Parse(data),
			"text" => TextConfigurationFileParser.Parse(data),
			_ => default
		};
		return _bus.SendAsync<SettingCreateCommand, long>(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
			           return task.Result;
		           }, cancellationToken);
	}
}