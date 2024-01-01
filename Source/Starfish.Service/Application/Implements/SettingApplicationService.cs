using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;
using Nerosoft.Starfish.UseCases;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置应用服务
/// </summary>
public class SettingApplicationService : BaseApplicationService, ISettingApplicationService
{
	/// <inheritdoc />
	public Task<List<SettingItemDto>> SearchAsync(SettingCriteria criteria, int page, int size, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingSearchUseCase>();
		var input = new SettingSearchInput(criteria, page, size);
		return useCase.ExecuteAsync(input, cancellationToken)
					  .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<int> CountAsync(SettingCriteria criteria, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingCountUseCase>();
		var input = new SettingCountInput(criteria);
		return useCase.ExecuteAsync(input, cancellationToken)
					  .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<SettingDetailDto> GetAsync(long id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingGetDetailUseCase>();
		var input = new SettingGetDetailInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
					  .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	public async Task<long> CreateAsync(SettingCreateDto data, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task DeleteAsync(long id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingDeleteUseCase>();
		var input = new SettingDeleteInput(id);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task PublishAsync(long id, SettingPublishDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingPublishUseCase>();
		var input = new SettingPublishInput(id, data);
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
}