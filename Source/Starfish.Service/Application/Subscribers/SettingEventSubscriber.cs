using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

public class SettingEventSubscriber
{
	private readonly IBus _bus;

	public SettingEventSubscriber(IBus bus)
	{
		_bus = bus;
	}
}