using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置归档事件订阅者
/// </summary>
public class SettingArchiveEventSubscriber : IHandler<SettingPublishedEvent>
{
	private readonly IBus _bus;

	public SettingArchiveEventSubscriber(IBus bus)
	{
		_bus = bus;
	}

	/// <summary>
	/// 处理配置发布事件
	/// </summary>
	/// <param name="message"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public Task HandleAsync(SettingPublishedEvent message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var command = new SettingArchiveCreateCommand
		{
			RootId = message.Id
		};
		
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}
}