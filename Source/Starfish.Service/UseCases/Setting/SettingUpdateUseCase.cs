using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Common;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface ISettingUpdateUseCase : INonOutputUseCase<SettingUpdateInput>;

public record SettingUpdateInput(long AppId, string Environment, string Format, SettingEditDto Data);

public class SettingUpdateUseCase : ISettingUpdateUseCase
{
	private readonly IBus _bus;
	private readonly IServiceProvider _provider;

	public SettingUpdateUseCase(IBus bus, IServiceProvider provider)
	{
		_bus = bus;
		_provider = provider;
	}

	public Task ExecuteAsync(SettingUpdateInput input, CancellationToken cancellationToken = default)
	{
		var format = input.Format?.Normalize(TextCaseType.Lower).Trim(TextTrimType.All);
		var parserName = format switch
		{
			"text/plain" => "text",
			"text/json" => "json",
			"" => throw new ArgumentNullException(Resources.IDS_ERROR_SETTING_DATA_FORMAT_REQUIRED),
			null => throw new ArgumentNullException(Resources.IDS_ERROR_SETTING_DATA_FORMAT_REQUIRED),
			_ => throw new InvalidOperationException(Resources.IDS_ERROR_SETTING_DATA_FORMAT_NOT_SUPPORTED)
		};

		var parser = _provider.GetNamedService<IConfigurationParser>(parserName);
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