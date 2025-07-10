using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;
using Nerosoft.Starfish.UseCases;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置应用服务
/// </summary>
public class ConfigurationApplicationService : BaseApplicationService, IConfigurationApplicationService
{
	public Task<List<ConfigurationDto>> QueryAsync(ConfigurationCriteria criteria, int skip, int count, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationQueryUseCase>();
		var input = new GenericQueryInput<ConfigurationCriteria>(criteria, skip, count);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	public Task<int> CountAsync(ConfigurationCriteria criteria, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationCountUseCase>();
		var input = new ConfigurationCountInput(criteria);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<List<ConfigurationItemDto>> GetItemListAsync(long id, string key, int skip, int count, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetConfigurationItemListUseCase>();
		var input = new GetConfigurationItemListInput(id, key, skip, count);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<int> GetItemCountAsync(long id, string key, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetConfigurationItemCountUseCase>();
		var input = new GetConfigurationItemCountInput(id, key);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<ConfigurationDto> GetDetailAsync(long id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetConfigurationDetailUseCase>();
		var input = new GetConfigurationDetailInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<long> CreateAsync(long teamId, ConfigurationEditDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationCreateUseCase>();
		var input = new ConfigurationCreateInput(teamId, data);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result, cancellationToken);
	}

	/// <inheritdoc />
	public async Task UpdateAsync(long id, ConfigurationEditDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationUpdateUseCase>();
		var input = new ConfigurationUpdateInput(id, data);
		await useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAsync(long id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationDeleteUseCase>();
		var input = new ConfigurationDeleteInput(id);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	public Task SetSecretAsync(long id, string secret, CancellationToken cancellationToken = default)
	{
		var input = new SetConfigurationSecretInput(id, secret);
		var useCase = LazyServiceProvider.GetRequiredService<ISetConfigurationSecretUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	public Task DisableAsync(long id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationDisableUseCase>();
		var input = new ConfigurationDisableInput(id);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	public Task EnableAsync(long id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationEnableUseCase>();
		var input = new ConfigurationEnableInput(id);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	public Task<long> AuthorizeAsync(long id, string name, string secret, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationAuthorizeUseCase>();
		var input = new ConfigurationAuthorizeInput(id, 0, name, secret);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Id, cancellationToken);
	}

	/// <inheritdoc />
	public Task UpdateValueAsync(long id, string key, string value, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationValueUpdateUseCase>();
		var input = new ConfigurationValueUpdateInput(id, key, value);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	public Task UpdateItemsAsync(long id, ConfigurationItemsUpdateDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationItemsUpdateUseCase>();
		var input = new ConfigurationItemsUpdateInput(id, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task PublishAsync(long id, ConfigurationPublishRequestDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationPublishUseCase>();
		var input = new ConfigurationPublishInput(id, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task<string> GetArchiveAsync(long id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetConfigurationArchiveUseCase>();
		var input = new GetConfigurationArchiveInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<string> GetItemsInTextAsync(long id, string format, CancellationToken cancellationToken = default)
	{
		var parser = LazyServiceProvider.GetRequiredService<IServiceProvider>()
		                                .GetKeyedService<IConfigurationParser>(format.Normalize(TextCaseType.Lower));
		return GetItemListAsync(id, string.Empty, 0, int.MaxValue, cancellationToken)
			.ContinueWith(task =>
			{
				task.WaitAndUnwrapException(cancellationToken);
				var items = task.Result.ToDictionary(t => t.Key, t => t.Value);
				var text = parser.InvertParse(items);
				return Cryptography.Base64.Encrypt(text);
			}, cancellationToken);
	}

	public Task PushRedisAsync(long id, ConfigurationPushRedisRequestDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationPushRedisUseCase>();
		var input = new ConfigurationPushRedisInput(id, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}
}