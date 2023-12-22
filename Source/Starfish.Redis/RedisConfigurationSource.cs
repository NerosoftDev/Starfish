using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Configuration.Redis;

public sealed class RedisConfigurationSource : IConfigurationSource
{
	/// <summary>
	/// The Redis connection string.
	/// </summary>
	[DisallowNull]
	public string ConnectionString { get; set; }

	/// <summary>
	/// Gets or sets the Redis database ID.
	/// </summary>
	public int Database { get; set; } = -1;

	/// <summary>
	/// Gets or sets the Redis key this source will read from.
	/// </summary>
	/// <remarks>
	/// The key is expected to be a hash.
	/// </remarks>
	public string Key { get; set; } = "appsettings";

	/// <summary>
	/// Determines whether the source will be loaded if the underlying file changes.
	/// </summary>
	public bool ReloadOnChange { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether keyspace events are enabled.
	/// </summary>
	public bool KeyspaceEnabled { get; set; }

	/// <inheritdoc />
	public IConfigurationProvider Build(IConfigurationBuilder builder)
	{
		return new RedisConfigurationProvider(this);
	}
}