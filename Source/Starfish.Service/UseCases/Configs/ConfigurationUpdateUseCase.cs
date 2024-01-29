using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface IConfigurationUpdateUseCase : INonOutputUseCase<ConfigurationUpdateInput>;

public record ConfigurationUpdateInput(string AppId, string Environment, string Format, ConfigurationEditDto Data);

public class ConfigurationUpdateUseCase : IConfigurationUpdateUseCase
{
	private readonly IBus _bus;
	private readonly IServiceProvider _provider;

	public ConfigurationUpdateUseCase(IBus bus, IServiceProvider provider)
	{
		_bus = bus;
		_provider = provider;
	}

	public Task ExecuteAsync(ConfigurationUpdateInput input, CancellationToken cancellationToken = default)
	{
		var format = input.Format?.Normalize(TextCaseType.Lower).Trim(TextTrimType.All);
		var parserName = format switch
		{
			"plain/text" => "text",
			"plain/json" => "json",
			"" => throw new ArgumentNullException(Resources.IDS_ERROR_CONFIG_DATA_FORMAT_REQUIRED),
			null => throw new ArgumentNullException(Resources.IDS_ERROR_CONFIG_DATA_FORMAT_REQUIRED),
			_ => throw new InvalidOperationException(Resources.IDS_ERROR_CONFIG_DATA_FORMAT_NOT_SUPPORTED)
		};

		var parser = _provider.GetKeyedService<IConfigurationParser>(parserName);
		var data = Cryptography.Base64.Decrypt(input.Data.Data);
		var command = new ConfigurationUpdateCommand(input.AppId, input.Environment)
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