using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;
using Nerosoft.Starfish.UseCases;

namespace Nerosoft.Starfish.Application;

internal class AdministratorApplicationService : BaseApplicationService, IAdministratorApplicationService
{
	public Task<List<AdministratorItemDto>> QueryAsync(AdministratorCriteria criteria, int skip, int count, CancellationToken cancellationToken = default)
	{
		var input = new GenericQueryInput<AdministratorCriteria>(criteria, skip, count);
		var useCase = LazyServiceProvider.GetRequiredService<IAdministratorQueryUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken).ContinueWith(task =>
		{
			task.WaitAndUnwrapException(cancellationToken);
			return task.Result.Result;
		}, cancellationToken);
	}

	public Task<int> CountAsync(AdministratorCriteria criteria, CancellationToken cancellationToken = default)
	{
		var input = new AdministratorCountInput(criteria);
		var useCase = LazyServiceProvider.GetRequiredService<IAdministratorCountUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken).ContinueWith(task =>
		{
			task.WaitAndUnwrapException(cancellationToken);
			return task.Result.Result;
		}, cancellationToken);
	}

	public Task AssignAsync(AdministratorAssignDto data, CancellationToken cancellationToken = default)
	{
		var input = new AdministratorAssignInput(data);
		var useCase = LazyServiceProvider.GetRequiredService<IAdministratorAssignUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.WaitAndUnwrapException(cancellationToken), cancellationToken);
	}

	public Task DeleteAsync(string userId, CancellationToken cancellationToken = default)
	{
		var input = new AdministratorDeleteInput(userId);
		var useCase = LazyServiceProvider.GetRequiredService<IAdministratorDeleteUseCase>();
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.WaitAndUnwrapException(cancellationToken), cancellationToken);
	}
}