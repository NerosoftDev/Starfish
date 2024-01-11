using System.Globalization;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Common;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface ISettingUpdateUseCase : INonOutputUseCase<SettingUpdateInput>;

public record SettingUpdateInput(long AppId, string Environment, SettingUpdateDto Data);

public class SettingUpdateUseCase : ISettingUpdateUseCase
{
	private readonly IBus _bus;
	private IServiceProvider _provider;

	public SettingUpdateUseCase(IBus bus, IServiceProvider provider)
	{
		_bus = bus;
		_provider = provider;
	}

	public Task ExecuteAsync(SettingUpdateInput input, CancellationToken cancellationToken = default)
	{
		var parser = _provider.GetNamedService<IConfigurationParser>(input.Data.Type.ToLower(CultureInfo.CurrentCulture));
		var data = Cryptography.Base64.Decrypt(input.Data.Data);
		var command = new SettingUpdateCommand(input.AppId, input.Environment)
		{
			Data = parser.Parse(data)
		};
		return _bus.SendAsync(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
		           }, cancellationToken);
	}
}