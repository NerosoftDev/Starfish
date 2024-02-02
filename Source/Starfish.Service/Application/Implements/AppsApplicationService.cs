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
	public Task<List<AppInfoItemDto>> QueryAsync(AppInfoCriteria criteria, int skip, int count, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoQueryInput(criteria, skip, count);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoQueryUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
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
	public Task<AppInfoDetailDto> GetAsync(string id, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoDetailInput(id);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoDetailUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<bool> AuthorizeAsync(string id, string secret, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoAuthorizeInput(id, secret);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoAuthorizeUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<string> CreateAsync(AppInfoCreateDto data, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoCreateInput(data);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoCreateUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Id, cancellationToken);
	}

	/// <inheritdoc />
	public Task UpdateAsync(string id, AppInfoUpdateDto data, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoUpdateInput(id, data);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoUpdateUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoDeleteInput(id);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoDeleteUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task EnableAsync(string id, CancellationToken cancellationToken = default)
	{
		var input = new ChangeAppInfoStatusInput(id, AppStatus.Enabled);
		var useCase = LazyServiceProvider.GetRequiredService<IChangeAppInfoStatusUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task DisableAsync(string id, CancellationToken cancellationToken = default)
	{
		var input = new ChangeAppInfoStatusInput(id, AppStatus.Disabled);
		var useCase = LazyServiceProvider.GetRequiredService<IChangeAppInfoStatusUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task SetSecretAsync(string id, string secret, CancellationToken cancellationToken = default)
	{
		var input = new AppInfoSetSecretInput(id, secret);
		var useCase = LazyServiceProvider.GetRequiredService<IAppInfoSetSecretUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken);
	}
}