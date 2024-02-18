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
		var input = new ConfigurationQueryInput(criteria, skip, count);
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
	public Task<List<ConfigurationItemDto>> GetItemListAsync(string id, int skip, int count, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetConfigurationItemListUseCase>();
		var input = new GetConfigurationItemListInput(id, skip, count);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<int> GetItemCountAsync(string id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetConfigurationItemCountUseCase>();
		var input = new GetConfigurationItemCountInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<ConfigurationDto> GetDetailAsync(string id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetConfigurationDetailUseCase>();
		var input = new GetConfigurationDetailInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<string> CreateAsync(string teamId, ConfigurationEditDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationCreateUseCase>();
		var input = new ConfigurationCreateInput(teamId, data);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result, cancellationToken);
	}

	/// <inheritdoc />
	public async Task UpdateAsync(string id, ConfigurationEditDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationUpdateUseCase>();
		var input = new ConfigurationUpdateInput(id, data);
		await useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationDeleteUseCase>();
		var input = new ConfigurationDeleteInput(id);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task UpdateValueAsync(string id, string key, string value, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationValueUpdateUseCase>();
		var input = new ConfigurationValueUpdateInput(id, key, value);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	public Task UpdateItemsAsync(string id, string format, string data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationItemsUpdateUseCase>();
		var input = new ConfigurationItemsUpdateInput(id, format, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task PublishAsync(string id, ConfigurationPublishDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IConfigurationPublishUseCase>();
		var input = new ConfigurationPublishInput(id, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task<string> GetArchiveAsync(string id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetConfigurationArchiveUseCase>();
		var input = new GetConfigurationArchiveInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<string> GetItemsInTextAsync(string id, string format, CancellationToken cancellationToken = default)
	{
		var parser = LazyServiceProvider.GetRequiredService<IServiceProvider>()
		                                .GetKeyedService<IConfigurationParser>(format.Normalize(TextCaseType.Lower));
		return GetItemListAsync(id, 0, int.MaxValue, cancellationToken)
			.ContinueWith(task =>
			{
				task.WaitAndUnwrapException(cancellationToken);
				var items = task.Result.ToDictionary(t => t.Key, t => t.Value);
				var text = parser.InvertParse(items);
				return Cryptography.Base64.Encrypt(text);
			}, cancellationToken);
	}

	public Task PushRedisAsync(string id, PushRedisRequestDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IPushRedisUseCase>();
		var input = new PushRedisInput(id, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}
}