using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

public class SettingNodeEventSubscriber : IHandler<SettingNodeRenamedEvent>
{
	private readonly IBus _bus;

	public SettingNodeEventSubscriber(IBus bus)
	{
		_bus = bus;
	}

	/// <inheritdoc />
	public Task HandleAsync(SettingNodeRenamedEvent message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var command = new SettingNodeSetKeyCommand(message.Id, message.OldName, message.NewName);
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}
}