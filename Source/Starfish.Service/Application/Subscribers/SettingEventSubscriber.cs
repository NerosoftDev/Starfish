using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

public class SettingEventSubscriber
{
	private readonly IBus _bus;

	public SettingEventSubscriber(IBus bus)
	{
		_bus = bus;
	}
}