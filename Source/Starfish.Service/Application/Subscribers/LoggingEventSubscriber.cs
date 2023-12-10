using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用日志收集处理器
/// </summary>
public sealed class LoggingEventSubscriber : IHandler<UserAuthSucceedEvent>, IHandler<UserAuthFailedEvent>
{
	private readonly IBus _bus;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	public LoggingEventSubscriber(IBus bus)
	{
		_bus = bus;
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
		var command = new CreateOperateLogCommand
		{
			Type = $"Auth.{message.AuthType}",
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
		var command = new CreateOperateLogCommand
		{
			Type = $"Auth.{message.AuthType}",
			Description = "认证失败",
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId, 
			Error = message.Error
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}
}