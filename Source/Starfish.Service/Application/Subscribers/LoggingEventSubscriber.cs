using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用日志收集处理器
/// </summary>
public sealed class LoggingEventSubscriber
{
	private const string MODULE_CONFIG = "config";
	
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
	/// 处理配置启用事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationEnabledEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Configuration>();
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "status",
			Description = string.Format(Resources.IDS_MESSAGE_LOGS_CONFIG_ENABLE, aggregate.Id, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理配置禁用事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationDisableEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Configuration>();
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "status",
			Description = string.Format(Resources.IDS_MESSAGE_LOGS_CONFIG_DISABLE, aggregate.Id, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理配置密钥重置事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationSecretChangedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Configuration>();
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "secret",
			Description = string.Format(Resources.IDS_MESSAGE_LOGS_CONFIG_RESET_SECRET, aggregate.Id, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理配置更新事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationUpdatedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Configuration>();
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "update",
			Description = string.Format(Resources.IDS_MESSAGE_LOGS_CONFIG_UPDATE, aggregate.Id, aggregate.Name),
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
		var description = string.Format(Resources.IDS_MESSAGE_LOGS_CONFIG_CREATE, @event.Configuration.Id, @event.Configuration.Name);

		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
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
		var description = string.Format(Resources.IDS_MESSAGE_LOGS_CONFIG_DELETE, aggregate.Id, aggregate.Name);

		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "delete",
			Description = description,
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理配置发布事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationPublishedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Configuration>();
		var description = string.Format(Resources.IDS_MESSAGE_LOGS_CONFIG_PUBLISH, aggregate.Id, aggregate.Name);

		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "publish",
			Description = description,
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}
}