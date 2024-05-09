using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用日志收集处理器
/// </summary>
internal sealed partial class LoggingEventSubscriber
{
	private readonly IBus _bus;

	public LoggingEventSubscriber(IBus bus)
	{
		_bus = bus;
	}
}