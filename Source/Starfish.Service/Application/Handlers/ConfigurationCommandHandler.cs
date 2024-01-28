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
                                     IHandler<ConfigurationRevisionCreateCommand>,
                                     IHandler<ConfigurationArchiveCreateCommand>
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
			business.AppId = message.AppId;
			business.Environment = message.Environment;
			business.Items = message.Data;
			_ = await business.SaveAsync(false, cancellationToken);
			return business.Id;
		}, context.Response);
	}

	public Task HandleAsync(ConfigurationUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<ConfigurationGeneralBusiness>(message.AppId, message.Environment, cancellationToken);
			business.Items = message.Data;
			_ = await business.SaveAsync(true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ConfigurationDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<ConfigurationGeneralBusiness>(message.AppId, message.Environment, cancellationToken);
			business.MarkAsDelete();
			await business.SaveAsync(false, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ConfigurationPublishCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<ConfigurationPublishBusiness>(message.AppId, message.Environment, cancellationToken);
		});
	}

	public Task HandleAsync(ConfigurationValueUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<ConfigurationGeneralBusiness>(message.AppId, message.Environment, cancellationToken);
			business.Key = message.Key;
			business.Value = message.Value;
			_ = await business.SaveAsync(true, cancellationToken);
		});
	}

	public Task HandleAsync(ConfigurationRevisionCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var argument = new ConfigurationRevisionArgument(message.Version, message.Comment, context.User?.Identity?.Name);
			await Factory.ExecuteAsync<ConfigurationRevisionBusiness>(message.AppId, message.Environment, argument, cancellationToken);
		});
	}

	public Task HandleAsync(ConfigurationArchiveCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<ConfigurationArchiveBusiness>(message.AppId, message.Environment, context.User?.Identity?.Name, cancellationToken);
		});
	}
}