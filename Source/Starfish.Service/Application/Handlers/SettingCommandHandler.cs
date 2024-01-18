using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用配置命令处理器
/// </summary>
public class SettingCommandHandler : CommandHandlerBase,
                                     IHandler<SettingCreateCommand>,
                                     IHandler<SettingUpdateCommand>,
                                     IHandler<SettingDeleteCommand>,
                                     IHandler<SettingPublishCommand>,
                                     IHandler<SettingValueUpdateCommand>,
                                     IHandler<SettingRevisionCreateCommand>,
                                     IHandler<SettingArchiveCreateCommand>
{
	public SettingCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
		: base(unitOfWork, factory)
	{
	}

	public Task HandleAsync(SettingCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.CreateAsync<SettingGeneralBusiness>(cancellationToken);
			business.AppId = message.AppId;
			business.Environment = message.Environment;
			business.Items = message.Data;
			_ = await business.SaveAsync(false, cancellationToken);
			return business.Id;
		}, context.Response);
	}

	public Task HandleAsync(SettingUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<SettingGeneralBusiness>(message.AppId, message.Environment, cancellationToken);
			business.Items = message.Data;
			_ = await business.SaveAsync(true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(SettingDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<SettingGeneralBusiness>(message.AppId, message.Environment, cancellationToken);
			business.MarkAsDelete();
			await business.SaveAsync(false, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(SettingPublishCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<SettingPublishBusiness>(message.AppId, message.Environment, cancellationToken);
		});
	}

	public Task HandleAsync(SettingValueUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<SettingGeneralBusiness>(message.AppId, message.Environment, cancellationToken);
			business.Key = message.Key;
			business.Value = message.Value;
			_ = await business.SaveAsync(true, cancellationToken);
		});
	}

	public Task HandleAsync(SettingRevisionCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var argument = new SettingRevisionArgument(message.Version, message.Comment, context.User?.Identity?.Name);
			await Factory.ExecuteAsync<SettingRevisionBusiness>(message.AppId, message.Environment, argument, cancellationToken);
		});
	}

	public Task HandleAsync(SettingArchiveCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<SettingArchiveBusiness>(message.AppId, message.Environment, context.User?.Identity?.Name, cancellationToken);
		});
	}
}