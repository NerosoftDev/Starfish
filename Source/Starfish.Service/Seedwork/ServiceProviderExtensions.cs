namespace Nerosoft.Starfish.Service;

internal static class ServiceProviderExtensions
{
	public static T GetServiceOrCreateInstance<T>(this IServiceProvider provider)
	{
		return ActivatorUtilities.GetServiceOrCreateInstance<T>(provider);
	}
}
