using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户应用服务实现
/// </summary>
public class UserApplicationService : BaseApplicationService, IUserApplicationService
{
	private UserRepository _userRepository;
	private UserRepository UserRepository => _userRepository ??= LazyServiceProvider.GetService<UserRepository>();

	/// <inheritdoc />
	public Task<int> CreateAsync(UserCreateDto model, CancellationToken cancellationToken = default)
	{
		var command = new UserCreateCommand();
		return Bus.SendAsync<UserCreateCommand, int>(command, cancellationToken)
		          .ContinueWith(task => task.Result, cancellationToken);
	}

	/// <inheritdoc />
	public Task UpdateAsync(int id, UserUpdateDto model, CancellationToken cancellationToken = default)
	{
		var command = new UserUpdateCommand();
		return Bus.SendAsync(command, cancellationToken);
	}

	/// <inheritdoc />
	public Task<List<UserItemDto>> SearchAsync(UserCriteria criteria, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<int> CountAsync(UserCriteria criteria, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<UserDetailDto> GetAsync(int id, CancellationToken cancellationToken = default)
	{
		return UserRepository.GetAsync(id, false, cancellationToken)
		                     .ContinueWith(task => TypeAdapter.ProjectedAs<UserDetailDto>(task.Result), cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
	{
		var command = new UserDeleteCommand(id);
		return Bus.SendAsync(command, cancellationToken);
	}
}