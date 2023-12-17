using System.Collections.Concurrent;
using System.Threading.Channels;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

public class ConnectionContainer
{
	public event EventHandler<ClientConnectedEventArgs> OnConnected;

	private static readonly ConcurrentDictionary<string, ConnectionInfo> _connections = new(StringComparer.OrdinalIgnoreCase);

	private readonly ISettingApplicationService _service;

	public ConnectionContainer(ISettingApplicationService service)
	{
		_service = service;

		this.OnConnected += OnClientConnected;
	}

	private async void OnClientConnected(object sender, ClientConnectedEventArgs e)
	{
		var raw = await _service.GetSettingRawAsync(e.AppId, e.Environment);
		var key = $"{e.AppId}-{e.Environment}";
		if (_connections.TryGetValue(key, out var connection))
		{
			await connection.Channel.Writer.WriteAsync(Tuple.Create(e.ConnectionId, raw));
		}
	}

	public ConnectionInfo GetOrAdd(long appId, string environment, string connectionId)
	{
		var key = $"{appId}-{environment}";

		var connection = _connections.AddOrUpdate(key, _ => ConnectionInfo.New(connectionId), (_, info) =>
		{
			info.AddClient(connectionId);
			return info;
		});

		OnConnected?.Invoke(this, new ClientConnectedEventArgs { AppId = appId, Environment = environment, ConnectionId = connectionId });
		return connection;
	}

	public void Remove(long appId, string environment, string connectionId)
	{
		var key = $"{appId}-{environment}";

		if (!_connections.TryGetValue(key, out var info))
		{
			return;
		}

		info.RemoveClient(connectionId);
		if (info.Clients.Count == 0)
		{
			_connections.TryRemove(key, out _);
		}
	}

	[Subscribe]
	public async Task HandleAsync(SettingArchiveUpdatedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<SettingArchive>();
		var key = $"{aggregate.AppId}-{aggregate.Environment}";
		if (_connections.TryGetValue(key, out var connection))
		{
			await connection.Channel.Writer.WriteAsync(Tuple.Create("*", aggregate.Data), cancellationToken);
		}
	}

	public class ClientConnectedEventArgs : EventArgs
	{
		public long AppId { get; set; }

		public string Environment { get; set; }

		public string ConnectionId { get; set; }
	}
}

public class ConnectionInfo
{
	public List<string> Clients { get; set; } = new();

	public Channel<Tuple<string, string>> Channel { get; set; }

	public static ConnectionInfo New(string connectionId)
	{
		var info = new ConnectionInfo
		{
			Channel = System.Threading.Channels.Channel.CreateUnbounded<Tuple<string, string>>()
		};
		info.AddClient(connectionId);

		return info;
	}

	public void AddClient(string connectionId)
	{
		Clients.Add(connectionId);
	}

	public void RemoveClient(string connectionId)
	{
		Clients.Remove(connectionId);
	}
}