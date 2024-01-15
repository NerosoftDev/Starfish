using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用日志收集处理器
/// </summary>
public sealed class LoggingEventSubscriber
{
	private readonly IBus _bus;
	private readonly IServiceProvider _provider;

	public LoggingEventSubscriber(IBus bus, IServiceProvider provider)
	{
		_bus = bus;
		_provider = provider;
	}

	/// <summary>
	/// 处理用户认证成功事件
	/// </summary>
	/// <param name="message"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="NotImplementedException"></exception>
	[Subscribe]
	public Task HandleAsync(UserAuthSucceedEvent message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var command = new OperateLogCreateCommand
		{
			Module = "auth",
			Type = message.AuthType,
			UserName = message.UserName,
			OperateTime = DateTime.Now,
			Description = Resources.IDS_MESSAGE_LOGS_AUTH_SUCCEED,
			RequestTraceId = context.RequestTraceId
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理用户认证失败事件
	/// </summary>
	/// <param name="message"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="NotImplementedException"></exception>
	[Subscribe]
	public Task HandleAsync(UserAuthFailedEvent message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var command = new OperateLogCreateCommand
		{
			Module = "auth",
			Type = message.AuthType,
			Description = Resources.IDS_MESSAGE_LOGS_AUTH_FAILED,
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			Error = message.Error
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理应用创建事件
	/// </summary>
	/// <param name="message"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	[Subscribe]
	public Task HandleAsync(AppInfoCreatedEvent message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = message.GetAggregate<AppInfo>();
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
	/// <param name="message"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(AppInfoEnabledEvent message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = message.GetAggregate<AppInfo>();
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
	/// <param name="message"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(AppInfoDisableEvent message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = message.GetAggregate<AppInfo>();
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

	/// <summary>
	/// 处理配置节点创建事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(SettingCreatedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var description = string.Format(Resources.IDS_MESSAGE_LOGS_SETTING_CREATE, @event.Setting.AppId, @event.Setting.Environment);

		var command = new OperateLogCreateCommand
		{
			Module = "setting",
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
	public Task HandleAsync(SettingDeletedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Setting>();
		var description = string.Format(Resources.IDS_MESSAGE_LOGS_SETTING_DELETE, aggregate.AppId, aggregate.Environment);

		var command = new OperateLogCreateCommand
		{
			Module = "setting",
			Type = "delete",
			Description = description,
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	[Subscribe]
	public async Task HandleAsync(SettingPublishedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var repository = _provider.GetService<ISettingRepository>();
		var setting = await repository.GetAsync(@event.AppId, false, [], cancellationToken);

		var description = string.Format(Resources.IDS_MESSAGE_LOGS_SETTING_PUBLISH, setting.AppId, setting.Environment);

		var command = new OperateLogCreateCommand
		{
			Module = "setting",
			Type = "publish",
			Description = description,
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};
		await _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}
}