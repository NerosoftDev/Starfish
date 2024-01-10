namespace Nerosoft.Starfish.Webapp;

internal static class ServiceLocator
{
	private static IServiceProvider Current => Singleton<HostAccessor>.Instance.ServiceProvider;

	/// <summary>
	/// 获取<typeparamref name="TService"/>的实例.
	/// </summary>
	/// <typeparam name="TService"></typeparam>
	/// <param name="newScope">是否创建新的Scope</param>
	/// <returns></returns>
	public static TService GetService<TService>(bool newScope = false)
	{
		var provider = newScope ? Current.CreateScope().ServiceProvider : Current;
		return GetService<TService>(provider);
	}

	/// <summary>
	/// 从指定的<see cref="IServiceProvider"/>获取<typeparamref name="TService"/>的实例.
	/// </summary>
	/// <typeparam name="TService"></typeparam>
	/// <param name="provider"></param>
	/// <returns></returns>
	public static TService GetService<TService>(IServiceProvider provider)
	{
		provider ??= Current;
		return provider.GetService<TService>();
	}

	/// <summary>
	/// 从<see cref="IServiceProvider"/>获取<typeparamref name="TService"/>的实例或创建一个新的实例.
	/// </summary>
	/// <typeparam name="TService"></typeparam>
	/// <param name="provider"></param>
	/// <returns></returns>
	public static TService GetServiceOrCreateInstance<TService>(IServiceProvider provider = null)
	{
		provider ??= Current;
		return ActivatorUtilities.GetServiceOrCreateInstance<TService>(provider);
	}

	public static object GetService(Type serviceType, bool newScope = false)
	{
		var provider = newScope ? Current.CreateScope().ServiceProvider : Current;
		return provider.GetService(serviceType);
	}

	public static object GetService(Type serviceType, IServiceProvider provider)
	{
		provider ??= Current;
		return provider.GetService(serviceType);
	}
}