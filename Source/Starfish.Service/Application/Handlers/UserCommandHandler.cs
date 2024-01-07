using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户命令处理器
/// </summary>
public sealed class UserCommandHandler : CommandHandlerBase,
                                         IHandler<UserCreateCommand>,
                                         IHandler<UserUpdateCommand>,
                                         IHandler<UserChangePasswordCommand>,
                                         IHandler<UserDeleteCommand>,
                                         IHandler<UserSetRoleCommand>
{
	/// <summary>
	/// 初始化<see cref="UserCommandHandler"/>.
	/// </summary>
	/// <param name="unitOfWork"></param>
	/// <param name="factory"></param>
	public UserCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
		: base(unitOfWork, factory)
	{
	}

	/// <inheritdoc/>
	public Task HandleAsync(UserCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.CreateAsync<UserGeneralBusiness>();
			business.UserName = message.Item1.UserName;
			business.Password = message.Item1.Password;
			business.NickName = message.Item1.NickName;
			business.Email = message.Item1.Email;
			business.Phone = message.Item1.Phone;
			business.Roles = message.Item1.Roles;
			await business.SaveAsync(false, cancellationToken);

			return business.Id;
		}, context.Response);
	}

	/// <inheritdoc />
	public Task HandleAsync(UserUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<UserGeneralBusiness>(message.Item1, cancellationToken);

			business.Email = message.Item2.Email;
			business.NickName = message.Item2.NickName;
			business.Roles = message.Item2.Roles;
			business.MarkAsUpdate();
			await business.SaveAsync(true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(UserChangePasswordCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<UserGeneralBusiness>(message.UserId, cancellationToken);

			business.Password = message.Password;
			business.MarkAsUpdate();
			await business.SaveAsync(true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(UserDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<UserGeneralBusiness>(message.Item1, cancellationToken);
			business.MarkAsDelete();
			await business.SaveAsync(true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(UserSetRoleCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<UserGeneralBusiness>(message.Item1, cancellationToken);

			business.Roles = message.Item2;

			business.MarkAsUpdate();

			await business.SaveAsync(true, cancellationToken);
		});
	}
}