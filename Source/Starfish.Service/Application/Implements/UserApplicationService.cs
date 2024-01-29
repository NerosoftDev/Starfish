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
	public Task<string> CreateAsync(UserCreateDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserCreateUseCase>();
		var input = new UserCreateInput(data);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task UpdateAsync(string id, UserUpdateDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserUpdateUseCase>();
		var input = new UserUpdateInput(id, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task<List<UserItemDto>> QueryAsync(UserCriteria criteria, int skip, int count, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserQueryUseCase>();
		var input = new UserSearchInput(criteria, skip, count);
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
	public Task<UserDetailDto> GetAsync(string id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserDetailUseCase>();
		var input = new UserDetailInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserDeleteUseCase>();
		var input = new UserDeleteInput(id);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	/// <inheritdoc />
	public Task SetRolesAsync(string id, List<string> roles, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserSetRoleUseCase>();
		var input = new UserSetRoleInput(id, roles);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	public Task ChangePasswordAsync(string oldPassword, string newPassword, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IChangePasswordUseCase>();
		var input = new ChangePasswordInput(oldPassword, newPassword);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	public Task ResetPasswordAsync(string id, string password, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IResetPasswordUseCase>();
		var input = new ResetPasswordInput(id, password);
		return useCase.ExecuteAsync(input, cancellationToken);
	}
}