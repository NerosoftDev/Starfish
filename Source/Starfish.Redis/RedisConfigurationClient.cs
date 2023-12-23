using Microsoft.Extensions.Primitives;
using StackExchange.Redis;

namespace Microsoft.Extensions.Configuration.Redis;

/// <summary>
/// The Redis configuration client.
/// </summary>
internal class RedisConfigurationClient : IAsyncDisposable
{
	private readonly int _database;
	private readonly string _key;
	private readonly bool _keyspaceEnabled;

	private readonly CountdownEvent _waitHandle = new(1);

	private Timer _timer;
	private ISubscriber _subscriber;

	/// <summary>
	/// Initializes a new instance of the <see cref="RedisConfigurationClient"/> class.
	/// </summary>
	/// <param name="connectionString"></param>
	/// <param name="database"></param>
	/// <param name="key"></param>
	/// <param name="keyspaceEnabled"></param>
	public RedisConfigurationClient(string connectionString, int database, string key, bool keyspaceEnabled)
	{
		_database = database;
		_key = key;
		_keyspaceEnabled = keyspaceEnabled;
		Connect(connectionString);
	}

	/// <summary>
	/// Gets or sets the Redis connection.
	/// </summary>
	private ConnectionMultiplexer Connection { get; set; }

	/// <summary>
	/// Gets or sets the cancellation source.
	/// </summary>
	private CancellationTokenSource CancellationSource { get; set; }

	/// <summary>
	/// Loads the configuration from Redis.
	/// </summary>
	/// <returns></returns>
	public async Task<Dictionary<string, string>> LoadAsync()
	{
		_waitHandle.Wait(TimeSpan.FromSeconds(30));

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

	/// <summary>
	/// Connects to Redis.
	/// </summary>
	/// <param name="connectionString"></param>
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
			_waitHandle.Signal();
		}
	}

	/// <summary>
	/// Reads the Redis value.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
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
		_waitHandle.Dispose();

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

	/// <summary>
	/// Watches the Redis key changes.
	/// </summary>
	/// <returns></returns>
	public IChangeToken Watch()
	{
		CancellationSource?.Dispose();
		CancellationSource = new CancellationTokenSource();
		var cancellationToken = new CancellationChangeToken(CancellationSource.Token);
		return cancellationToken;
	}
}