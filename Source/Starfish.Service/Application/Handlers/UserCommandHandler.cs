using Microsoft.Extensions.Logging;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Core;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户命令处理器
/// </summary>
public sealed class UserCommandHandler : CommandHandlerBase,
										 IHandler<UserCreateCommand>,
										 IHandler<UserUpdateCommand>,
										 IHandler<UserChangePasswordCommand>,
										 IHandler<UserDeleteCommand>
{
	private readonly UserRepository _repository;

	/// <summary>
	/// 初始化<see cref="UserCommandHandler"/>.
	/// </summary>
	/// <param name="userRepository"></param>
	/// <param name="unitOfWork"></param>
	/// <param name="factory"></param>
	/// <param name="logger"></param>
	public UserCommandHandler(UserRepository userRepository, IUnitOfWorkManager unitOfWork, IObjectFactory factory, ILoggerFactory logger)
		: base(unitOfWork, factory, logger)
	{
		_repository = userRepository;
	}

	/// <inheritdoc/>
	public Task HandleAsync(UserCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var isUserNameAvailable = await CheckUserNameAsync(message.UserName);
			if (!isUserNameAvailable)
			{
				throw new ConflictException(string.Format(Resources.IDS_ERROR_USERNAME_UNAVAILABLE, message.UserName));
			}

			var user = User.Create(message.UserName, message.Password, message.Email, message.Roles);
			await _repository.InsertAsync(user, true, cancellationToken);
			return user.Id;
		}, context.Response);

		Task<bool> CheckUserNameAsync(string userName)
		{
			return _repository.ExistsAsync(t => t.UserName == userName, cancellationToken)
							  .ContinueWith(task => !task.Result, cancellationToken);
		}
	}

	/// <inheritdoc />
	public Task HandleAsync(UserUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var user = await _repository.GetAsync(message.UserId, true, cancellationToken);
			if (user == null)
			{
				throw new UserNotFoundException(message.UserId);
			}

			user.SetEmail(message.Email);
			user.SetNickName(message.NickName);
			user.SetRoles(message.Roles);

			await _repository.UpdateAsync(user, true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(UserChangePasswordCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var user = await _repository.GetAsync(message.UserId, true, cancellationToken);
			if (user == null)
			{
				throw new UserNotFoundException(message.UserId);
			}

			user.ChangePassword(message.Password);

			await _repository.UpdateAsync(user, true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(UserDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var user = await _repository.GetAsync(message.Item1, true, cancellationToken);
			if (user == null)
			{
				throw new UserNotFoundException(message.Item1);
			}

			await _repository.DeleteAsync(user, true, cancellationToken);
		});
	}
}