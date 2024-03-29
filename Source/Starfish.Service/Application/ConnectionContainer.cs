﻿using System.Collections.Concurrent;
using System.Threading.Channels;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

public class ConnectionContainer
{
	public event EventHandler<ClientConnectedEventArgs> OnConnected;

	private static readonly ConcurrentDictionary<string, ConnectionChannel> _connections = new(StringComparer.OrdinalIgnoreCase);

	private readonly IConfigurationApplicationService _service;

	public ConnectionContainer(IConfigurationApplicationService service)
	{
		_service = service;

		OnConnected += OnClientConnected;
	}

	private async void OnClientConnected(object sender, ClientConnectedEventArgs e)
	{
		var raw = await _service.GetArchiveAsync(e.ConfigId);
		if (string.IsNullOrEmpty(raw))
		{
			return;
		}

		if (_connections.TryGetValue(e.ConfigId, out var connection))
		{
			await connection.Channel.Writer.WriteAsync(Tuple.Create(e.ConnectionId, raw));
		}
	}

	public ConnectionChannel GetOrAdd(string configId, string connectionId, string connectionType)
	{
		var channel = _connections.GetOrAdd(configId, _ => ConnectionChannel.New(configId));

		channel.AddClient(connectionId, connectionType);

		OnConnected?.Invoke(this, new ClientConnectedEventArgs { ConfigId = configId, ConnectionId = connectionId });
		return channel;
	}

	public List<ConnectionInfo> GetConnections(string configId)
	{
		return _connections.TryGetValue(configId, out var info) ? info.Connections : null;
	}

	public List<ConnectionInfo> GetConnections()
	{
		return _connections.SelectMany(t => t.Value.Connections ?? []).ToList();
	}

	public void Remove(string configId, string connectionId)
	{
		if (!_connections.TryGetValue(configId, out var info))
		{
			return;
		}

		info.RemoveClient(connectionId);
		if (info.Connections.Count == 0)
		{
			_connections.TryRemove(configId, out _);
		}
	}

	[Subscribe]
	public async Task HandleAsync(ConfigurationArchiveUpdatedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<ConfigurationArchive>();
		if (_connections.TryGetValue(aggregate.Id, out var connection))
		{
			await connection.Channel.Writer.WriteAsync(Tuple.Create("*", aggregate.Data), cancellationToken);
		}
	}

	public class ClientConnectedEventArgs : EventArgs
	{
		public string ConfigId { get; set; }

		public string ConnectionId { get; set; }

		public DateTime ConnectedTime { get; set; }
	}
}

public class ConnectionChannel
{
	private ConnectionChannel(string configId)
	{
		ConfigId = configId;
		Connections = [];
	}

	public string ConfigId { get; }

	public List<ConnectionInfo> Connections { get; private set; }

	public Channel<Tuple<string, string>> Channel { get; private init; }

	public static ConnectionChannel New(string configId)
	{
		var info = new ConnectionChannel(configId)
		{
			Channel = System.Threading.Channels.Channel.CreateUnbounded<Tuple<string, string>>()
		};

		return info;
	}

	public void AddClient(string connectionId, string type)
	{
		Connections.Add(new ConnectionInfo
		{
			ConfigurationId = ConfigId,
			ConnectionId = connectionId,
			ConnectionType = type,
			ConnectedTime = DateTime.Now
		});
	}

	public void RemoveClient(string connectionId)
	{
		Connections.RemoveAll(t => t.ConnectionId == connectionId);
	}
}

public class ConnectionInfo
{
	public string ConfigurationId { get; set; }

	public string ConnectionId { get; set; }

	public string ConnectionType { get; set; }

	public DateTime ConnectedTime { get; set; }
}