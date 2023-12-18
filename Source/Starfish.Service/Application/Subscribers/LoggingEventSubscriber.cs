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
			Description = "认证成功",
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
			Description = "认证失败",
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
			Module = "appinfo",
			Type = "create",
			Description = $"创建应用 {aggregate.Code}({aggregate.Name})",
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
			Module = "appinfo",
			Type = "enable",
			Description = $"启用应用 {aggregate.Code}({aggregate.Name})",
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
			Type = "disable",
			Description = $"禁用应用 {aggregate.Code}({aggregate.Name})",
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
	public Task HandleAsync(SettingNodeCreatedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var description = $"创建配置节点({@event.Node.Type}) {@event.Node.Name}, AppId: {@event.Node.AppId}, AppCode: {@event.Node.AppCode}, Environment: {@event.Node.Environment}";

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
	public Task HandleAsync(SettingNodeDeletedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<SettingNode>();
		var description = $"删除配置节点({aggregate.Type}) {aggregate.Name}, AppId: {aggregate.AppId}, AppCode: {aggregate.AppCode}, Environment: {aggregate.Environment}";

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

	/// <summary>
	/// 处理配置节点重命名事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(SettingNodeRenamedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<SettingNode>();
		var command = new OperateLogCreateCommand
		{
			Module = "setting",
			Type = "rename",
			Description = $"节点{@event.OldName}重命名为{@event.NewName}, AppId: {aggregate.AppId}, AppCode: {aggregate.AppCode}, Environment: {aggregate.Environment}",
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	[Subscribe]
	public async Task HandleAsync(SettingPublishedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var repository = _provider.GetService<ISettingNodeRepository>();
		var node = await repository.GetAsync(@event.Id, false, [], cancellationToken);
		var command = new OperateLogCreateCommand
		{
			Module = "setting",
			Type = "publish",
			Description = $"发布配置({@event.Version})，AppId: {node.AppId}, AppCode: {node.AppCode}, Environment: {node.Environment}",
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};
		await _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}
}