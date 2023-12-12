using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;
using Nerosoft.Starfish.UseCases;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用信息服务接口
/// </summary>
public class AppsApplicationService : BaseApplicationService, IAppsApplicationService
{
	/// <inheritdoc />
	public Task<List<AppInfoItemDto>> SearchAsync(AppInfoCriteria criteria, int page, int size, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoSearchInput(criteria, page, size);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoSearchUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Items, cancellationToken);
	}

	/// <inheritdoc />
	public Task<int> CountAsync(AppInfoCriteria criteria, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoCountInput(criteria);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoCountUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Count, cancellationToken);
	}

	/// <inheritdoc />
	public Task<AppInfoDetailDto> GetAsync(long id, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoDetailInput(id);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoDetailUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<bool> AuthorizeAsync(string code, string secret, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoAuthorizeInput(code, secret);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoAuthorizeUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<long> CreateAsync(AppInfoCreateDto model, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoCreateInput(model);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoCreateUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Id, cancellationToken);
	}

	/// <inheritdoc />
	public Task UpdateAsync(long id, AppInfoUpdateDto model, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoUpdateInput(id, model);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoUpdateUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAsync(long id, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoDeleteInput(id);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoDeleteUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task EnableAsync(long id, CancellationToken cancellationToken = default)
	{
		var input = new ChangeAppInfoStatusInput(id, AppStatus.Enabled);
		var useCase = LazyServiceProvider.GetRequiredService<IChangeAppInfoStatusUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task DisableAsync(long id, CancellationToken cancellationToken = default)
	{
		var input = new ChangeAppInfoStatusInput(id, AppStatus.Disabled);
		var useCase = LazyServiceProvider.GetRequiredService<IChangeAppInfoStatusUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken);
	}
}