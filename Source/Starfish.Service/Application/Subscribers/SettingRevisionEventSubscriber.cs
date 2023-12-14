using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置版本事件订阅者
/// </summary>
public class SettingRevisionEventSubscriber : IHandler<SettingPublishedEvent>
{
	/// <summary>
	/// 处理配置发布事件
	/// </summary>
	/// <param name="message"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public Task HandleAsync(SettingPublishedEvent message, MessageContext context, CancellationToken cancellationToken = new CancellationToken())
	{
		throw new NotImplementedException();
	}
}