using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用信息命令处理器
/// </summary>
public class AppInfoCommandHandler : CommandHandlerBase,
                                     IHandler<AppInfoCreateCommand>,
                                     IHandler<AppInfoUpdateCommand>,
                                     IHandler<AppInfoDeleteCommand>,
                                     IHandler<ChangeAppStatusCommand>,
                                     IHandler<AppInfoSetSecretCommand>
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="unitOfWork"></param>
	/// <param name="factory"></param>
	public AppInfoCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
		: base(unitOfWork, factory)
	{
	}

	/// <inheritdoc />
	public Task HandleAsync(AppInfoCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.CreateAsync<AppInfoGeneralBusiness>(cancellationToken);

			business.TeamId = message.Item1.TeamId;
			business.Name = message.Item1.Name;
			business.Code = message.Item1.Code;
			business.Description = message.Item1.Description;
			business.Secret = message.Item1.Secret;

			business.MarkAsInsert();

			_ = await business.SaveAsync(false, cancellationToken);

			return business.Id;
		}, context.Response);
	}

	/// <inheritdoc />
	public Task HandleAsync(AppInfoUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<AppInfoGeneralBusiness>(message.Item1, cancellationToken);

			business.Code = message.Item2.Code;
			business.Name = message.Item2.Name;
			business.Description = message.Item2.Description;

			business.MarkAsUpdate();

			_ = await business.SaveAsync(true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(AppInfoDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<AppInfoGeneralBusiness>(message.Item1, cancellationToken);

			business.MarkAsDelete();

			_ = await business.SaveAsync(true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ChangeAppStatusCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<AppInfoStatusBusiness>(message.Id, message.Status, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(AppInfoSetSecretCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<AppInfoSecretBusiness>(message.Id, message.Secret, cancellationToken);
		});
	}
}