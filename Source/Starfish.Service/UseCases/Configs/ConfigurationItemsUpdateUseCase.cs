using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

public interface IConfigurationItemsUpdateUseCase : INonOutputUseCase<ConfigurationItemsUpdateInput>;

public record ConfigurationItemsUpdateInput(string Id, string Format, string Data);

public class ConfigurationItemsUpdateUseCase : IConfigurationItemsUpdateUseCase
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
		var format = input.Format?.Normalize(TextCaseType.Lower).Trim(TextTrimType.All);
		var parserName = format switch
		{
			Constants.Configuration.FormatText => "text",
			Constants.Configuration.FormatJson => "json",
			"" => throw new ArgumentNullException(Resources.IDS_ERROR_CONFIG_DATA_FORMAT_REQUIRED),
			null => throw new ArgumentNullException(Resources.IDS_ERROR_CONFIG_DATA_FORMAT_REQUIRED),
			_ => throw new InvalidOperationException(Resources.IDS_ERROR_CONFIG_DATA_FORMAT_NOT_SUPPORTED)
		};

		var parser = _provider.GetKeyedService<IConfigurationParser>(parserName);
		var data = Cryptography.Base64.Decrypt(input.Data);
		var command = new ConfigurationItemsUpdateCommand(input.Id, parser.Parse(data));

		return _bus.SendAsync(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
		           }, cancellationToken);
	}
}