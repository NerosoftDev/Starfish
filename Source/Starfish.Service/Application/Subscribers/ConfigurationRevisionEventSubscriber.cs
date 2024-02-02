using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置版本事件订阅者
/// </summary>
public class ConfigurationRevisionEventSubscriber : IHandler<ConfigurationPublishedEvent>
{
	private readonly IBus _bus;

	public ConfigurationRevisionEventSubscriber(IBus bus)
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
	public Task HandleAsync(ConfigurationPublishedEvent message, MessageContext context, CancellationToken cancellationToken = default)
	{
		var command = new ConfigurationRevisionCreateCommand(message.AppId, message.Environment)
		{
			Version = message.Version,
			Comment = message.Comment
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}
}