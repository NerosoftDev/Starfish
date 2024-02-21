using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IConfigurationItemsUpdateUseCase : INonOutputUseCase<ConfigurationItemsUpdateInput>;

internal record ConfigurationItemsUpdateInput(string Id, ConfigurationItemsUpdateDto Data);

internal class ConfigurationItemsUpdateUseCase : IConfigurationItemsUpdateUseCase
{
	private readonly IBus _bus;
	private readonly IServiceProvider _provider;

	public ConfigurationItemsUpdateUseCase(IBus bus, IServiceProvider provider)
	{
		_bus = bus;
		_provider = provider;
	}

	public Task ExecuteAsync(ConfigurationItemsUpdateInput input, CancellationToken cancellationToken = default)
	{
		var format = input.Data.Type?.Normalize(TextCaseType.Lower).Trim(TextTrimType.All);
		var parserName = format switch
		{
			Constants.Configuration.FormatText => "text",
			Constants.Configuration.FormatJson => "json",
			"" => throw new ArgumentNullException(Resources.IDS_ERROR_CONFIG_DATA_FORMAT_REQUIRED),
			null => throw new ArgumentNullException(Resources.IDS_ERROR_CONFIG_DATA_FORMAT_REQUIRED),
			_ => throw new InvalidOperationException(Resources.IDS_ERROR_CONFIG_DATA_FORMAT_NOT_SUPPORTED)
		};

		var parser = _provider.GetKeyedService<IConfigurationParser>(parserName);
		var data = Cryptography.Base64.Decrypt(input.Data.Data);
		var command = new ConfigurationItemsUpdateCommand(input.Id, input.Data.Mode, parser.Parse(data));

		return _bus.SendAsync(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
		           }, cancellationToken);
	}
}