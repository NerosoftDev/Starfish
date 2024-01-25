using CommunityToolkit.Mvvm.Messaging;
using Nerosoft.Starfish.Webapp.Rest;
using Refit;

namespace Nerosoft.Starfish.Webapp;

internal static class TaskExtensions
{
	public static async Task Guard(this Task task, Func<Exception, Task> handler = null)
	{
		try
		{
			await task;
		}
		catch (Exception exception)
		{
			WeakReferenceMessenger.Default.Send(exception, Constants.Message.ExceptionThrown);
			if (handler != null)
			{
				await handler(exception);
			}
		}
	}

	public static async Task Guard<TResult>(this Task<TResult> task, Func<TResult, Task> next, Func<Exception, Task> handler = null)
	{
		try
		{
			var result = await task;
			if (next != null)
			{
				await next(result);
			}
		}
		catch (Exception exception)
		{
			WeakReferenceMessenger.Default.Send(exception, Constants.Message.ExceptionThrown);
			if (handler != null)
			{
				await handler(exception);
			}
		}
	}
}