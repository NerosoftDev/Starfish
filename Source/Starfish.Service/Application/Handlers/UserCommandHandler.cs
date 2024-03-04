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
                                         IHandler<ChangePasswordCommand>,
                                         IHandler<UserDeleteCommand>
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
			var business = await Factory.CreateAsync<UserGeneralBusiness>(cancellationToken);
			business.UserName = message.UserName;
			business.Password = message.Password;
			business.NickName = message.NickName;
			business.Email = message.Email;
			business.Phone = message.Phone;
			business.IsAdmin = message.IsAdmin;
			business.Reserved = message.Reserved;
			business.MarkAsInsert();
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
			business.MarkAsUpdate();
			await business.SaveAsync(true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ChangePasswordCommand message, MessageContext context, CancellationToken cancellationToken = default)
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
}