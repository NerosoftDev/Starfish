namespace Nerosoft.Starfish.Common;

/// <summary>
/// Provides some helper methods to work with async methods.
/// </summary>
public static class AsyncHelper
{
	public static TResult RunSync<TResult>(Func<Task<TResult>> func)
	{
		var oldContext = SynchronizationContext.Current;
		var context = new ExclusiveSynchronizationContext();
		SynchronizationContext.SetSynchronizationContext(context);
		TResult result = default;
		context.Post(async _ =>
		{
			try
			{
				result = await func();
			}
			catch (Exception exception)
			{
				context.Exception = exception;
				throw;
			}
			finally
			{
				context.EndMessageLoop();
			}
		}, null);
		context.BeginMessageLoop();
		SynchronizationContext.SetSynchronizationContext(oldContext);
		return result;
	}

	public static void RunSync(Func<Task> func)
	{
		var oldContext = SynchronizationContext.Current;
		var context = new ExclusiveSynchronizationContext();
		SynchronizationContext.SetSynchronizationContext(context);
		context.Post(async _ =>
		{
			try
			{
				await func();
			}
			catch (Exception exception)
			{
				context.Exception = exception;
				throw;
			}
			finally
			{
				context.EndMessageLoop();
			}
		}, null);
		context.BeginMessageLoop();

		SynchronizationContext.SetSynchronizationContext(oldContext);
	}

	private class ExclusiveSynchronizationContext : SynchronizationContext
	{
		private bool done;
		public Exception Exception { get; set; }
		readonly AutoResetEvent workItemsWaiting = new(false);
		readonly Queue<Tuple<SendOrPostCallback, object>> items = new();

		public override void Send(SendOrPostCallback d, object state)
		{
			throw new NotSupportedException("We cannot send to our same thread");
		}

		public override void Post(SendOrPostCallback callback, object state)
		{
			lock (items)
			{
				items.Enqueue(Tuple.Create(callback, state));
			}
			workItemsWaiting.Set();
		}

		public void EndMessageLoop()
		{
			Post(_ => done = true, null);
		}

		public void BeginMessageLoop()
		{
			if (done)
			{
				return;
			}
			while (!done)
			{
				Tuple<SendOrPostCallback, object> task = null;
				lock (items)
				{
					if (items.Count > 0)
					{
						task = items.Dequeue();
					}
				}
				if (task != null)
				{
					task.Item1(task.Item2);
					if (Exception != null) // the method threw an exception
					{
						throw new AggregateException("AsyncHelper.Run method threw an exception.", Exception);
					}
				}
				else
				{
					workItemsWaiting.WaitOne();
				}
			}
		}

		public override SynchronizationContext CreateCopy()
		{
			return this;
		}
	}
}
