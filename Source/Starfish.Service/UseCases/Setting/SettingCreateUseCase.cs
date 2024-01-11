using System.Globalization;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Common;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface ISettingCreateUseCase : IUseCase<SettingCreateInput, long>;

public record SettingCreateInput(long AppId, string Environment, SettingCreateDto Data) : IUseCaseInput;

public class SettingCreateUseCase : ISettingCreateUseCase
{
	private readonly IBus _bus;
	private readonly IServiceProvider _provider;

	public SettingCreateUseCase(IBus bus, IServiceProvider provider)
	{
		_bus = bus;
		_provider = provider;
	}

	public Task<long> ExecuteAsync(SettingCreateInput input, CancellationToken cancellationToken = default)
	{
		var parser = _provider.GetNamedService<IConfigurationParser>(input.Data.Type.ToLower(CultureInfo.CurrentCulture));
		var data = Cryptography.Base64.Decrypt(input.Data.Data);
		var command = new SettingCreateCommand(input.AppId, input.Environment)
		{
			Data = parser.Parse(data)
		};
		return _bus.SendAsync<SettingCreateCommand, long>(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
			           return task.Result;
		           }, cancellationToken);
	}
}