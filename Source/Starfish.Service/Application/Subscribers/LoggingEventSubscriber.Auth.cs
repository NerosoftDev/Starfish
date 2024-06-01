using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

internal partial class LoggingEventSubscriber
{
	private const string MODULE_AUTH = "auth";

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
			Module = MODULE_AUTH,
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
			Module = MODULE_AUTH,
			Type = @event.AuthType,
			Description = Resources.IDS_MESSAGE_LOGS_AUTH_FAILED,
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			Error = @event.Error
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}
}
