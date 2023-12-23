using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration.Redis;

/// <summary>
/// The configuration provider using Redis.
/// </summary>
public sealed class RedisConfigurationProvider : ConfigurationProvider, IAsyncDisposable
{
	private readonly RedisConfigurationClient _client;

	private readonly IDisposable _changeToken;

	/// <summary>
	/// Initializes a new instance of the <see cref="RedisConfigurationProvider"/> class.
	/// </summary>
	/// <param name="source"></param>
	public RedisConfigurationProvider(RedisConfigurationSource source)
	{
		_client = new RedisConfigurationClient(source.ConnectionString, source.Database, source.Key, source.KeyspaceEnabled);

		if (source.ReloadOnChange)
		{
			_changeToken = ChangeToken.OnChange(_client.Watch, Load);
		}
	}

	/// <inheritdoc />
	public override void Load()
	{
		Data = _client.LoadAsync()
		              .GetAwaiter()
		              .GetResult();
	}

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		_changeToken?.Dispose();
		await _client.DisposeAsync();
	}
}