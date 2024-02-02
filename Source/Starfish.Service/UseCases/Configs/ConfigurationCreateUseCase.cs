using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface IConfigurationCreateUseCase : IUseCase<ConfigurationCreateInput, long>;

public record ConfigurationCreateInput(string AppId, string Environment, string Format, ConfigurationEditDto Data) : IUseCaseInput;

public class ConfigurationCreateUseCase : IConfigurationCreateUseCase
{
	private readonly IBus _bus;
	private readonly IServiceProvider _provider;

	public ConfigurationCreateUseCase(IBus bus, IServiceProvider provider)
	{
		_bus = bus;
		_provider = provider;
	}

	public Task<long> ExecuteAsync(ConfigurationCreateInput input, CancellationToken cancellationToken = default)
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
		var data = Cryptography.Base64.Decrypt(input.Data.Data);
		var command = new ConfigurationCreateCommand(input.AppId, input.Environment)
		{
			Data = parser.Parse(data)
		};
		return _bus.SendAsync<ConfigurationCreateCommand, long>(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
			           return task.Result;
		           }, cancellationToken);
	}
}