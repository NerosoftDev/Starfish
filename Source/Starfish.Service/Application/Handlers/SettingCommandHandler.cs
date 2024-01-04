﻿using Nerosoft.Euonia.Bus;
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
                                     IHandler<SettingPublishCommand>
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

	public Task HandleAsync(SettingUpdateCommand message, MessageContext context, CancellationToken cancellationToken = new CancellationToken())
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<SettingGeneralBusiness>(message.Id, cancellationToken);
			business.Items = message.Data;
			_ = await business.SaveAsync(true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(SettingDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<SettingGeneralBusiness>(message.Item1, cancellationToken);
			business.MarkAsDelete();
			await business.SaveAsync(false, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(SettingPublishCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<SettingPublishBusiness>(message.Id, cancellationToken);
		});
	}
}