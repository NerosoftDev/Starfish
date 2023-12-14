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
	public Task<List<SettingNodeItemDto>> SearchAsync(SettingNodeCriteria criteria, int page, int size, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingNodeSearchUseCase>();
		var input = new SettingNodeSearchInput(criteria, page, size);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<int> CountAsync(SettingNodeCriteria criteria, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingNodeCountUseCase>();
		var input = new SettingNodeCountInput(criteria);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<SettingNodeDetailDto> GetAsync(long id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingNodeGetDetailUseCase>();
		var input = new SettingNodeGetDetailInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<long> CreateRootNodeAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingRootNodeCreateUseCase>();
		var input = new SettingRootNodeCreationInput(appId, environment);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Id, cancellationToken);
	}

	/// <inheritdoc />
	public Task<long> CreateLeafNodeAsync(long parentId, SettingNodeType type, SettingNodeCreateDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingLeafNodeCreateUseCase>();
		var input = new SettingLeafNodeCreationInput(parentId, type, data);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(t => t.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task UpdateAsync(long id, SettingNodeUpdateDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingNodeUpdateUseCase>();
		var input = new SettingNodeUpdateInput(id, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task RenameAsync(long id, string name, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingNodeRenameUseCase>();
		var input = new SettingNodeRenameInput(id, name);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAsync(long id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingNodeDeleteUseCase>();
		var input = new SettingNodeDeleteInput(id);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task PublishAsync(long id, SettingNodePublishDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetRequiredService<ISettingNodePublishUseCase>();
		var input = new SettingNodePublishInput(id, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}
}