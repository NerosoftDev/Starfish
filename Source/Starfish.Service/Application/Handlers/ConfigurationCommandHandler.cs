using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用配置命令处理器
/// </summary>
public class ConfigurationCommandHandler : CommandHandlerBase,
                                           IHandler<ConfigurationCreateCommand>,
                                           IHandler<ConfigurationUpdateCommand>,
                                           IHandler<ConfigurationDeleteCommand>,
                                           IHandler<ConfigurationPublishCommand>,
                                           IHandler<ConfigurationValueUpdateCommand>,
                                           IHandler<ConfigurationItemsUpdateCommand>,
                                           IHandler<ConfigurationSetSecretCommand>,
                                           IHandler<ConfigurationDisableCommand>,
                                           IHandler<ConfigurationEnableCommand>
{
	public ConfigurationCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
		: base(unitOfWork, factory)
	{
	}

	public Task HandleAsync(ConfigurationCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.CreateAsync<ConfigurationGeneralBusiness>(cancellationToken);
			business.TeamId = message.TeamId;
			business.Name = message.Name;
			business.Description = message.Description;
			business.Secret = message.Secret;

			_ = await business.SaveAsync(false, cancellationToken);

			return business.Id;
		}, context.Response);
	}

	public Task HandleAsync(ConfigurationUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<ConfigurationGeneralBusiness>(message.Id, cancellationToken);
			business.Name = message.Name;
			business.Description = message.Description;
			business.Secret = message.Secret;
			_ = await business.SaveAsync(true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ConfigurationDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<ConfigurationGeneralBusiness>(message.Id, cancellationToken);
			business.MarkAsDelete();
			await business.SaveAsync(false, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ConfigurationPublishCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<ConfigurationPublishBusiness>(message.Id, message.Version, message.Comment, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ConfigurationValueUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<ConfigurationItemsBusiness>(message.Id, message.Key, message.Value, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ConfigurationItemsUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<ConfigurationItemsBusiness>(message.Id, message.Items, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ConfigurationSetSecretCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<ConfigurationSecretBusiness>(message.Id, message.Secret, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ConfigurationDisableCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<ConfigurationStatusBusiness>(message.Id, false, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ConfigurationEnableCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<ConfigurationStatusBusiness>(message.Id, true, cancellationToken);
		});
	}
}