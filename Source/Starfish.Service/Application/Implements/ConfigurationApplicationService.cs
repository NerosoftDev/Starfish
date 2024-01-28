using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Common;
using Nerosoft.Starfish.Transit;
using Nerosoft.Starfish.UseCases;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置应用服务
/// </summary>
public class ConfigurationApplicationService : BaseApplicationService, IConfigurationApplicationService
{
	/// <inheritdoc />
	public Task<List<ConfigurationItemDto>> GetItemListAsync(long appId, string environment, int skip, int count, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetConfigurationItemListUseCase>();
		var input = new GetConfigurationItemListInput(appId, environment, skip, count);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<int> GetItemCountAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetConfigurationItemCountUseCase>();
		var input = new GetConfigurationItemCountInput(appId, environment);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<ConfigurationDetailDto> GetDetailAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetConfigurationDetailUseCase>();
		var input = new GetConfigurationDetailInput(appId, environment);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<long> CreateAsync(long appId, string environment, string format, ConfigurationEditDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationCreateUseCase>();
		var input = new ConfigurationCreateInput(appId, environment, format, data);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result, cancellationToken);
	}

	/// <inheritdoc />
	public async Task UpdateAsync(long appId, string environment, string format, ConfigurationEditDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationUpdateUseCase>();
		var input = new ConfigurationUpdateInput(appId, environment, format, data);
		await useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationDeleteUseCase>();
		var input = new ConfigurationDeleteInput(appId, environment);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task UpdateAsync(long appId, string environment, string key, string value, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationValueUpdateUseCase>();
		var input = new ConfigurationValueUpdateInput(appId, environment, key, value);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task PublishAsync(long appId, string environment, ConfigurationPublishDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationPublishUseCase>();
		var input = new ConfigurationPublishInput(appId, environment, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task<string> GetArchiveAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetConfigurationRawUseCase>();
		var input = new GetConfigurationRawInput(appId, environment);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<string> GetItemsInTextAsync(long appId, string environment, string format, CancellationToken cancellationToken = default)
	{
		var parser = LazyServiceProvider.GetRequiredService<IServiceProvider>()
		                                .GetKeyedService<IConfigurationParser>(format.Normalize(TextCaseType.Lower));
		return GetItemListAsync(appId, environment, 0, int.MaxValue, cancellationToken)
			.ContinueWith(task =>
			{
				task.WaitAndUnwrapException(cancellationToken);
				var items = task.Result.ToDictionary(t => t.Key, t => t.Value);
				var text = parser.InvertParse(items);
				return Cryptography.Base64.Encrypt(text);
			}, cancellationToken);
	}

	public Task PushRedisAsync(long appId, string environment, PushRedisRequestDto data, CancellationToken cancellationToken = default)
	{ 
		var useCase = LazyServiceProvider.GetRequiredService<IPushRedisUseCase>();
		var input = new PushRedisInput(appId, environment, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}
}