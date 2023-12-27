using System.Globalization;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;

namespace Nerosoft.Starfish.Etcd;

internal partial class EtcdClient : IEtcdClient, IDisposable
{
	private const string RANGE_END_STRING = "\x00";

	private const string INSECURE_PREFIX = "http://";
	private const string SECURE_PREFIX = "https://";

	private const string STATIC_HOSTS_PREFIX = "static://";
	private const string DNS_PREFIX = "dns://";
	private const string ALTERNATE_DNS_PREFIX = "discovery-srv://";

	private const string DEFAULT_SERVER_NAME = "etcd-server";

	private readonly EtcdConnection _connection;

	// https://learn.microsoft.com/en-us/aspnet/core/grpc/retries?view=aspnetcore-6.0#configure-a-grpc-retry-policy
	private static readonly MethodConfig _defaultGrpcMethodConfig = new()
	{
		Names = { MethodName.Default },
		RetryPolicy = new RetryPolicy
		{
			MaxAttempts = 5,
			InitialBackoff = TimeSpan.FromSeconds(1),
			MaxBackoff = TimeSpan.FromSeconds(5),
			BackoffMultiplier = 1.5,
			RetryableStatusCodes = { StatusCode.Unavailable }
		}
	};

	// https://github.com/grpc/proposal/blob/master/A6-client-retries.md#throttling-retry-attempts-and-hedged-rpcs
	private static readonly RetryThrottlingPolicy _defaultRetryThrottlingPolicy = new()
	{
		MaxTokens = 10,
		TokenRatio = 0.1
	};

	public EtcdClient(string connectionString, int port = 2379, string serverName = DEFAULT_SERVER_NAME, Action<GrpcChannelOptions> configureChannelOptions = null, Interceptor[] interceptors = null)
	{
		// Param check
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			throw new ArgumentNullException(nameof(connectionString));
		}

		// Param sanitization

		interceptors ??= [];

		if (connectionString.StartsWith(ALTERNATE_DNS_PREFIX, StringComparison.InvariantCultureIgnoreCase))
		{
			connectionString = connectionString.Substring(ALTERNATE_DNS_PREFIX.Length);
			connectionString = DNS_PREFIX + connectionString;
		}

		// Connection Configuration
		var options = new GrpcChannelOptions
		{
			ServiceConfig = new ServiceConfig
			{
				MethodConfigs = { _defaultGrpcMethodConfig },
				RetryThrottling = _defaultRetryThrottlingPolicy,
				LoadBalancingConfigs = { new RoundRobinConfig() },
			}
		};

		configureChannelOptions?.Invoke(options);

		// Channel Configuration
		GrpcChannel channel;
		if (connectionString.StartsWith(DNS_PREFIX, StringComparison.InvariantCultureIgnoreCase))
		{
			channel = GrpcChannel.ForAddress(connectionString, options);
		}
		else
		{
			var hosts = connectionString.Split(',');
			List<Uri> nodes = [];

			for (int i = 0; i < hosts.Length; i++)
			{
				string host = hosts[i];
				if (host.Split(':').Length < 3)
				{
					host += $":{Convert.ToString(port, CultureInfo.InvariantCulture)}";
				}

				if (!(host.StartsWith(INSECURE_PREFIX, StringComparison.InvariantCultureIgnoreCase) || host.StartsWith(SECURE_PREFIX, StringComparison.InvariantCultureIgnoreCase)))
				{
					host = options.Credentials == ChannelCredentials.Insecure ? $"{INSECURE_PREFIX}{host}" : $"{SECURE_PREFIX}{host}";
				}

				nodes.Add(new Uri(host));
			}

			var factory = new StaticResolverFactory(addr => nodes.Select(i => new BalancerAddress(i.Host, i.Port)).ToArray());
			//var services = new ServiceCollection();
			//services.AddSingleton<ResolverFactory>(factory);
			//options.ServiceProvider = services.BuildServiceProvider();

			channel = GrpcChannel.ForAddress($"{STATIC_HOSTS_PREFIX}{serverName}", options);
		}

		var callInvoker = interceptors is { Length: > 0 } ? channel.Intercept(interceptors) : channel.CreateCallInvoker();

		_connection = new EtcdConnection
		{
			StoreClient = new Store.StoreClient(callInvoker),
			WatchClient = new Watch.WatchClient(callInvoker),
			AuthClient = new Auth.AuthClient(callInvoker)
		};
	}

	/// <summary>
	/// Converts RangeResponse to Dictionary
	/// </summary>
	/// <returns>IDictionary corresponding the RangeResponse</returns>
	/// <param name="resp">RangeResponse received from etcd server</param>
	private static IDictionary<string, string> RangeRespondToDictionary(RangeResponse resp)
	{
		Dictionary<string, string> resDictionary = new();
		foreach (KeyValue kv in resp.Kvs)
		{
			resDictionary.Add(kv.Key.ToStringUtf8(), kv.Value.ToStringUtf8());
		}

		return resDictionary;
	}

	/// <summary>
	/// Gets the range end for prefix
	/// </summary>
	/// <returns>The range end for prefix</returns>
	/// <param name="prefixKey">Prefix key</param>
	public static string GetRangeEnd(string prefixKey)
	{
		if (prefixKey.Length == 0)
		{
			return RANGE_END_STRING;
		}

		StringBuilder rangeEnd = new(prefixKey);
		rangeEnd[index: rangeEnd.Length - 1] = ++rangeEnd[rangeEnd.Length - 1];
		return rangeEnd.ToString();
	}

	/// <summary>
	/// Gets the byte string for range requests
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static ByteString GetStringByteForRangeRequests(string key) => key.Length == 0 ? ByteString.CopyFrom(0) : ByteString.CopyFromUtf8(key);

	/// <summary>
	/// Generic helper for performing actions an a connection.
	/// Gets the connection from the <seealso cref="Balancer"/>
	/// Also implements a retry mechanism if the calling methods returns an <seealso cref="RpcException"/> with the <seealso cref="StatusCode"/> <seealso cref="StatusCode.Unavailable"/>
	/// </summary>
	/// <typeparam name="TResponse">The type of the response that is returned from the call to etcd</typeparam>
	/// <param name="etcdCallFunc">The function to perform actions with the <seealso cref="EtcdConnection"/> object</param>
	/// <returns>The response from the the <paramref name="etcdCallFunc"/></returns>
	private TResponse CallEtcd<TResponse>(Func<EtcdConnection, TResponse> etcdCallFunc) => etcdCallFunc.Invoke(_connection);

	/// <summary>
	/// Generic helper for performing actions an a connection.
	/// Gets the connection from the <seealso cref="Balancer"/>
	/// Also implements a retry mechanism if the calling methods returns an <seealso cref="RpcException"/> with the <seealso cref="StatusCode"/> <seealso cref="StatusCode.Unavailable"/>
	/// </summary>
	/// <typeparam name="TResponse">The type of the response that is returned from the call to etcd</typeparam>
	/// <param name="etcdCallFunc">The function to perform actions with the <seealso cref="EtcdConnection"/> object</param>
	/// <returns>The response from the the <paramref name="etcdCallFunc"/></returns>
	private Task<TResponse> CallEtcdAsync<TResponse>(Func<EtcdConnection, Task<TResponse>> etcdCallFunc) => etcdCallFunc.Invoke(_connection);

	/// <summary>
	/// Generic helper for performing actions an a connection.
	/// Gets the connection from the <seealso cref="Balancer"/>
	/// Also implements a retry mechanism if the calling methods returns an <seealso cref="RpcException"/> with the <seealso cref="StatusCode"/> <seealso cref="StatusCode.Unavailable"/>
	/// </summary>
	/// <param name="etcdCallFunc">The function to perform actions with the <seealso cref="EtcdConnection"/> object</param>
	/// <returns>The response from the the <paramref name="etcdCallFunc"/></returns>
	private Task CallEtcdAsync(Func<EtcdConnection, Task> etcdCallFunc) => etcdCallFunc.Invoke(_connection);

	#region IDisposable Implementation

	private bool _disposed; // To detect redundant calls

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				// TODO: dispose managed state (managed objects).
			}

			// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
			// TODO: set large fields to null.
			_disposed = true;
		}
	}

	// This code added to correctly implement the disposable pattern.
	public void Dispose()
	{
		// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		Dispose(true);
		// TODO: uncomment the following line if the finalizer is overridden above.
		GC.SuppressFinalize(this);
	}

	#endregion
}