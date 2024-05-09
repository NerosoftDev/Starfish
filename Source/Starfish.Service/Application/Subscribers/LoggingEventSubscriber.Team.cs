using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

internal partial class LoggingEventSubscriber
{
	private const string MODULE_TEAM = "team";

	[Subscribe]
	public async Task HandleAsync(TeamCreatedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Team>();
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_TEAM,
			Type = "create",
			OperateTime = DateTime.Now,
			UserName = context.User?.Identity?.Name,
			RequestTraceId = context.RequestTraceId
		};
		await _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	[Subscribe]
	public async Task HandleAsync(TeamMemberAppendedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Team>();
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_TEAM,
			Type = "member.append",
			OperateTime = DateTime.Now,
			UserName = context.User?.Identity?.Name,
			RequestTraceId = context.RequestTraceId
		};
		await _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	[Subscribe]
	public async Task HandleAsync(TeamMemberRemovedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Team>();
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_TEAM,
			Type = "member.remove",
			OperateTime = DateTime.Now,
			UserName = context.User?.Identity?.Name,
			RequestTraceId = context.RequestTraceId
		};
		await _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}
}
