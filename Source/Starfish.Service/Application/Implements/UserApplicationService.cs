using System.Security.Authentication;
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
	public Task UpdateAsync(long id, UserUpdateDto data, CancellationToken cancellationToken = default)
	{
		var command = new UserUpdateCommand(id, data);
		// Use the bus to send the command for updating the user
		return Bus.SendAsync(command, cancellationToken);
	}

	/// <inheritdoc />
	public Task<List<UserItemDto>> QueryAsync(UserCriteria criteria, int skip, int count, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserQueryUseCase>();
		var input = new GenericQueryInput<UserCriteria>(criteria, skip, count);
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
	public Task<UserDetailDto> GetAsync(long id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserDetailUseCase>();
		var input = new UserDetailInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAsync(long id, CancellationToken cancellationToken = default)
	{
		var command = new UserDeleteCommand(id);
		return Bus.SendAsync(command, cancellationToken);
	}

	public Task ChangePasswordAsync(string password, CancellationToken cancellationToken = default)
	{
		if (User?.IsAuthenticated != true)
		{
			throw new AuthenticationException("User is not authenticated.");
		}

		var command = new ChangePasswordCommand(User.GetUserIdOfInt64(), password, "change");
		return Bus.SendAsync(command, cancellationToken);
	}

	public Task ResetPasswordAsync(long id, string password, CancellationToken cancellationToken = default)
	{
		var command = new ChangePasswordCommand(id, password, "reset");
		return Bus.SendAsync(command, cancellationToken);
	}

	public Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<IUserInitializeUseCase>();
		return useCase.ExecuteAsync(cancellationToken);
	}
}