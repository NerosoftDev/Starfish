namespace Microsoft.Extensions.Configuration.Redis;

public static class RedisConfigurationExtensions
{
	/// <summary>
	/// Adds a Redis configuration source to <paramref name="builder"/>.
	/// </summary>
	/// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
	/// <param name="connectionString">The Redis connection string.</param>
	/// <param name="key">The Redis key to read configuration data.</param>
	/// <param name="database">The Redis database ID.</param>
	/// <param name="keyspaceEnabled">A value indicating whether keyspace events are enabled. Use <code>CONFIG GET notify-keyspace-events</code>to check.</param>
	/// <param name="reloadOnChange">Whether the configuration should be reloaded if the value changes.</param>
	/// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
	public static IConfigurationBuilder AddRedis(this IConfigurationBuilder builder, string connectionString, string key, int database = -1, bool keyspaceEnabled = true, bool reloadOnChange = true)
	{
		ArgumentNullException.ThrowIfNull(builder, nameof(builder));
		ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));
		ArgumentNullException.ThrowIfNull(key, nameof(key));
		return builder.AddRedis(source =>
		{
			source.ConnectionString = connectionString;
			source.Key = key;
			source.Database = database;
			source.KeyspaceEnabled = keyspaceEnabled;
			source.ReloadOnChange = reloadOnChange;
		});
	}

	/// <summary>
	/// Adds a Redis configuration source to <paramref name="builder"/>.
	/// </summary>
	/// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
	/// <param name="configureSource">Configures the source.</param>
	/// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
	public static IConfigurationBuilder AddRedis(this IConfigurationBuilder builder, Action<RedisConfigurationSource> configureSource)
	{
		return builder.Add(configureSource);
	}
}