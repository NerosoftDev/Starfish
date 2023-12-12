using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用日志收集处理器
/// </summary>
public sealed class LoggingEventSubscriber : IHandler<UserAuthSucceedEvent>,
                                             IHandler<UserAuthFailedEvent>,
                                             IHandler<AppInfoCreatedEvent>,
                                             IHandler<AppInfoEnabledEvent>,
                                             IHandler<AppInfoDisableEvent>
{
	private readonly IBus _bus;
	private readonly IServiceProvider _provider;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	/// <param name="provider"></param>
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
	public Task HandleAsync(AppInfoCreatedEvent message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var user = _provider.GetService<UserPrincipal>();
		var aggregate = message.GetAggregate<AppInfo>();
		var command = new OperateLogCreateCommand
		{
			Module = "appinfo",
			Type = "create",
			Description = $"创建应用 {aggregate.Code}({aggregate.Name})",
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = user?.Username
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
	public Task HandleAsync(AppInfoEnabledEvent message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var user = _provider.GetService<UserPrincipal>();
		var aggregate = message.GetAggregate<AppInfo>();
		var command = new OperateLogCreateCommand
		{
			Module = "appinfo",
			Type = "enable",
			Description = $"启用应用 {aggregate.Code}({aggregate.Name})",
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = user?.Username
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
	public Task HandleAsync(AppInfoDisableEvent message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var user = _provider.GetService<UserPrincipal>();
		var aggregate = message.GetAggregate<AppInfo>();
		var command = new OperateLogCreateCommand
		{
			Module = "appinfo",
			Type = "disable",
			Description = $"禁用应用 {aggregate.Code}({aggregate.Name})",
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = user?.Username
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}
}