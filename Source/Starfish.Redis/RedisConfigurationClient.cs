using Microsoft.Extensions.Primitives;
using StackExchange.Redis;

namespace Microsoft.Extensions.Configuration.Redis;

internal class RedisConfigurationClient : IAsyncDisposable
{
	private readonly int _database;
	private readonly string _key;
	private readonly bool _keyspaceEnabled;

	private readonly SemaphoreSlim _semaphoreSlim = new(0, 1);

	private Timer _timer;
	private ISubscriber _subscriber;

	public RedisConfigurationClient(string connectionString, int database, string key, bool keyspaceEnabled)
	{
		_database = database;
		_key = key;
		_keyspaceEnabled = keyspaceEnabled;
		Connect(connectionString);
	}

	private ConnectionMultiplexer Connection { get; set; }

	private CancellationTokenSource CancellationSource { get; set; }

	public async Task<Dictionary<string, string>> LoadAsync()
	{
		await _semaphoreSlim.WaitAsync();

		if (Connection == null)
		{
			return [];
		}

		return await Connection.GetDatabase(_database)
		                       .HashGetAllAsync(_key)
		                       .ContinueWith(task =>
		                       {
			                       return task.Result.ToDictionary(x => x.Name.ToString(), x => ReadRedisValue(x.Value));
		                       });
	}

	private async void Connect(string connectionString)
	{
		try
		{
			Connection = await ConnectionMultiplexer.ConnectAsync(connectionString);

			if (_keyspaceEnabled)
			{
				var channel = RedisChannel.Pattern($"__keyspace@{_database}__:{_key}");

				_subscriber = Connection.GetSubscriber();
				await _subscriber.SubscribeAsync(channel, (_, _) =>
				{
					CancellationSource?.Cancel();
				});
			}
			else
			{
				_timer = new Timer(_ => CancellationSource?.Cancel(), null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
			}
		}
		catch
		{
			// ignored
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	private static string ReadRedisValue(RedisValue value)
	{
		if (value.IsNull)
		{
			return null;
		}

		return value.IsNullOrEmpty ? string.Empty : value.ToString();
	}

	public async ValueTask DisposeAsync()
	{
		if (Connection != null)
		{
			await Connection.CloseAsync();
			await Connection.DisposeAsync();
		}

		if (_timer != null)
		{
			await _timer.DisposeAsync();
		}

		if (_subscriber != null)
		{
			await _subscriber.UnsubscribeAllAsync();
			_subscriber = null;
		}

		CancellationSource?.Dispose();
	}

	public IChangeToken Watch()
	{
		CancellationSource?.Dispose();
		CancellationSource = new CancellationTokenSource();
		var cancellationToken = new CancellationChangeToken(CancellationSource.Token);
		return cancellationToken;
	}
}