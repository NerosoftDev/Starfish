using System.Globalization;

namespace Nerosoft.Starfish.Common;
/// <summary>
/// Provides some helper methods to work with async methods.
/// </summary>
public static class AsyncHelper
{
	private static readonly TaskFactory _factory = new(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

	public static TResult RunSync<TResult>(Func<Task<TResult>> func)
	{
		var cultureUi = CultureInfo.CurrentUICulture;
		var culture = CultureInfo.CurrentCulture;
		return _factory.StartNew(() =>
		{
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = cultureUi;
			return func();
		}).Unwrap().GetAwaiter().GetResult();
	}

	public static void RunSync(Func<Task> func)
	{
		var cultureUi = CultureInfo.CurrentUICulture;
		var culture = CultureInfo.CurrentCulture;
		_factory.StartNew(() =>
		{
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = cultureUi;
			return func();
		}).Unwrap().GetAwaiter().GetResult();
	}
}
