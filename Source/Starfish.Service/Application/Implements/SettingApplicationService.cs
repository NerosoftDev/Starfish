using System.Globalization;
using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Common;
using Nerosoft.Starfish.Transit;
using Nerosoft.Starfish.UseCases;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置应用服务
/// </summary>
public class SettingApplicationService : BaseApplicationService, ISettingApplicationService
{
	/// <inheritdoc />
	public Task<List<SettingItemDto>> GetItemListAsync(long appId, string environment, int page, int size, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetSettingItemListUseCase>();
		var input = new GetSettingItemListInput(appId, environment, page, size);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<int> GetItemCountAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetSettingItemCountUseCase>();
		var input = new GetSettingItemCountInput(appId, environment);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<SettingDetailDto> GetDetailAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetSettingDetailUseCase>();
		var input = new GetSettingDetailInput(appId, environment);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	public Task<long> CreateAsync(long appId, string environment, SettingCreateDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingCreateUseCase>();
		var input = new SettingCreateInput(appId, environment, data);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result, cancellationToken);
	}

	public async Task UpdateAsync(long appId, string environment, SettingUpdateDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingUpdateUseCase>();
		var input = new SettingUpdateInput(appId, environment, data);
		await useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingDeleteUseCase>();
		var input = new SettingDeleteInput(appId, environment);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	public Task UpdateAsync(long appId, string environment, string key, string value, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task PublishAsync(long appId, string environment, SettingPublishDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingPublishUseCase>();
		var input = new SettingPublishInput(appId, environment, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task<string> GetSettingRawAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<IGetSettingRawUseCase>();
		var input = new GetSettingRawUseCaseInput(appId, environment);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	public Task<string> GetItemsInTextAsync(long appId, string environment, string type, CancellationToken cancellationToken = default)
	{
		var parser = LazyServiceProvider.GetRequiredService<IServiceProvider>().GetNamedService<IConfigurationParser>(type.ToLower(CultureInfo.CurrentCulture));
		return GetItemListAsync(appId, environment, 1, int.MaxValue, cancellationToken)
			.ContinueWith(task =>
			{
				task.WaitAndUnwrapException(cancellationToken);
				var items = task.Result.ToDictionary(t => t.Key, t => t.Value);
				var text = parser.InvertParse(items);
				return Cryptography.Base64.Encrypt(text);
			}, cancellationToken);
	}
}