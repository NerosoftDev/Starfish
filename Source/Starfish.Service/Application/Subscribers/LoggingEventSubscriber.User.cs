using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

internal sealed partial class LoggingEventSubscriber
{
	private const string MODULE_USER = "user";

	[Subscribe]
	public async Task HandleAsync(UserCreatedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_USER,
			Type = "create",
			UserName = @event.UserName,
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId
		};
		await _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	[Subscribe]
	public async Task HandleAsync(UserPasswordChangedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<User>();
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_USER,
			Type = "password",
			UserName = aggregate.UserName,
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId
		};
		await _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}
}
