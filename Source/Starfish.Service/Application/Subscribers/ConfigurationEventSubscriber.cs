using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

public class ConfigurationEventSubscriber
{
	private readonly IBus _bus;

	public ConfigurationEventSubscriber(IBus bus)
	{
		_bus = bus;
	}
}