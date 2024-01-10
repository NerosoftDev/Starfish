namespace Nerosoft.Starfish.Webapp;

internal class HostAccessor
{
	public IServiceProvider ServiceProvider { get; init; }

	public IConfiguration Configuration { get; init; }
}