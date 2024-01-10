using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;
using Nerosoft.Starfish.UseCases;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户应用服务实现
/// </summary>
public class UserApplicationService : BaseApplicationService, IUserApplicationService
{
	/// <inheritdoc />
	public Task<int> CreateAsync(UserCreateDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserCreateUseCase>();
		var input = new UserCreateInput(data);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task UpdateAsync(int id, UserUpdateDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserUpdateUseCase>();
		var input = new UserUpdateInput(id, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task<List<UserItemDto>> SearchAsync(UserCriteria criteria, int page, int size, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserSearchUseCase>();
		var input = new UserSearchInput(criteria, page, size);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task<int> CountAsync(UserCriteria criteria, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserCountUseCase>();
		return useCase.ExecuteAsync(criteria, cancellationToken);
	}

	/// <inheritdoc />
	public Task<UserDetailDto> GetAsync(int id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserDetailUseCase>();
		var input = new UserDetailInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserDeleteUseCase>();
		var input = new UserDeleteInput(id);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task SetRolesAsync(int id, List<string> roles, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserSetRoleUseCase>();
		var input = new UserSetRoleInput(id, roles);
		return useCase.ExecuteAsync(input, cancellationToken);
	}
}