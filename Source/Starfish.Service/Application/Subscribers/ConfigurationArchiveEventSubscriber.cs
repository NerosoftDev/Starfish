using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置归档事件订阅者
/// </summary>
public class ConfigurationArchiveEventSubscriber : IHandler<ConfigurationPublishedEvent>
{
	private readonly IBus _bus;

	public ConfigurationArchiveEventSubscriber(IBus bus)
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
		var command = new ConfigurationArchiveCreateCommand
		{
			AppId = message.AppId,
			Environment = message.Environment,
		};
		
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}
}