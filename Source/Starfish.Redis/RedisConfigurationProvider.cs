using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration.Redis;

/// <summary>
/// 
/// </summary>
public sealed class RedisConfigurationProvider : ConfigurationProvider, IAsyncDisposable
{
	private readonly RedisConfigurationClient _client;

	private readonly IDisposable _changeToken;

	public RedisConfigurationProvider(RedisConfigurationSource source)
	{
		_client = new RedisConfigurationClient(source.ConnectionString, source.Database, source.Key, source.KeyspaceEnabled);

		if (source.ReloadOnChange)
		{
			_changeToken = ChangeToken.OnChange(_client.Watch, Load);
		}
	}

	public override void Load()
	{
		Data = _client.LoadAsync()
		              .GetAwaiter()
		              .GetResult();
	}

	public async ValueTask DisposeAsync()
	{
		_changeToken?.Dispose();
		await _client.DisposeAsync();
	}
}