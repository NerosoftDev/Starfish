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

		context.Post(Callback, null);
		context.BeginMessageLoop();
		SynchronizationContext.SetSynchronizationContext(oldContext);
		return result;

		async void Callback(object _)
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
		}
	}

	public static void RunSync(Func<Task> func)
	{
		var oldContext = SynchronizationContext.Current;
		var context = new ExclusiveSynchronizationContext();
		SynchronizationContext.SetSynchronizationContext(context);

		context.Post(Callback, null);
		context.BeginMessageLoop();

		SynchronizationContext.SetSynchronizationContext(oldContext);
		return;

		async void Callback(object _)
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
		}
	}

	private class ExclusiveSynchronizationContext : SynchronizationContext
	{
		private bool _done;
		public Exception Exception { get; set; }
		private readonly AutoResetEvent _workItemsWaiting = new(false);
		private readonly Queue<Tuple<SendOrPostCallback, object>> _items = new();

		public override void Send(SendOrPostCallback d, object state)
		{
			throw new NotSupportedException("We cannot send to our same thread");
		}

		public override void Post(SendOrPostCallback callback, object state)
		{
			lock (_items)
			{
				_items.Enqueue(Tuple.Create(callback, state));
			}

			_workItemsWaiting.Set();
		}

		public void EndMessageLoop()
		{
			Post(_ => _done = true, null);
		}

		public void BeginMessageLoop()
		{
			if (_done)
			{
				return;
			}

			while (!_done)
			{
				Tuple<SendOrPostCallback, object> task = null;
				lock (_items)
				{
					if (_items.Count > 0)
					{
						task = _items.Dequeue();
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
					_workItemsWaiting.WaitOne();
				}
			}
		}

		public override SynchronizationContext CreateCopy()
		{
			return this;
		}
	}
}