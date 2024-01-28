using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用日志收集处理器
/// </summary>
public sealed class LoggingEventSubscriber
{
	private readonly IBus _bus;

	public LoggingEventSubscriber(IBus bus)
	{
		_bus = bus;
	}

	/// <summary>
	/// 处理用户认证成功事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="NotImplementedException"></exception>
	[Subscribe]
	public Task HandleAsync(UserAuthSucceedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var command = new OperateLogCreateCommand
		{
			Module = "auth",
			Type = @event.AuthType,
			UserName = @event.UserName,
			OperateTime = DateTime.Now,
			Description = Resources.IDS_MESSAGE_LOGS_AUTH_SUCCEED,
			RequestTraceId = context.RequestTraceId
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理用户认证失败事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="NotImplementedException"></exception>
	[Subscribe]
	public Task HandleAsync(UserAuthFailedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var command = new OperateLogCreateCommand
		{
			Module = "auth",
			Type = @event.AuthType,
			Description = Resources.IDS_MESSAGE_LOGS_AUTH_FAILED,
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			Error = @event.Error
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理应用创建事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	[Subscribe]
	public Task HandleAsync(AppInfoCreatedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<AppInfo>();
		var command = new OperateLogCreateCommand
		{
			Module = "apps",
			Type = "create",
			Description = string.Format(Resources.IDS_MESSAGE_LOGS_APPS_CREATE, aggregate.Code, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理应用启用事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(AppInfoEnabledEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<AppInfo>();
		var command = new OperateLogCreateCommand
		{
			Module = "apps",
			Type = "status",
			Description = string.Format(Resources.IDS_MESSAGE_LOGS_APPS_ENABLE, aggregate.Code, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理应用禁用事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(AppInfoDisableEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<AppInfo>();
		var command = new OperateLogCreateCommand
		{
			Module = "appinfo",
			Type = "status",
			Description = string.Format(Resources.IDS_MESSAGE_LOGS_APPS_DISABLE, aggregate.Code, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	[Subscribe]
	public Task HandleAsync(AppInfoSecretChangedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<AppInfo>();
		var command = new OperateLogCreateCommand
		{
			Module = "apps",
			Type = "secret",
			Description = string.Format(Resources.IDS_MESSAGE_LOGS_APPS_RESET_SECRET, aggregate.Code, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	[Subscribe]
	public Task HandleAsync(AppInfoUpdatedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<AppInfo>();
		var command = new OperateLogCreateCommand
		{
			Module = "apps",
			Type = "update",
			Description = string.Format(Resources.IDS_MESSAGE_LOGS_APPS_UPDATE, aggregate.Code, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理配置节点创建事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationCreatedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var description = string.Format(Resources.IDS_MESSAGE_LOGS_CONFIG_CREATE, @event.Configuration.AppId, @event.Configuration.Environment);

		var command = new OperateLogCreateCommand
		{
			Module = "config",
			Type = "create",
			Description = description,
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理配置节点删除事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationDeletedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Configuration>();
		var description = string.Format(Resources.IDS_MESSAGE_LOGS_CONFIG_DELETE, aggregate.AppId, aggregate.Environment);

		var command = new OperateLogCreateCommand
		{
			Module = "config",
			Type = "delete",
			Description = description,
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	[Subscribe]
	public Task HandleAsync(ConfigurationPublishedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var description = string.Format(Resources.IDS_MESSAGE_LOGS_CONFIG_PUBLISH, @event.AppId, @event.Environment);

		var command = new OperateLogCreateCommand
		{
			Module = "config",
			Type = "publish",
			Description = description,
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	
}