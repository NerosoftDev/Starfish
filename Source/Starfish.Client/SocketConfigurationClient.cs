using System.Net.WebSockets;

namespace Nerosoft.Starfish.Client;

internal class SocketConfigurationClient : IConfigurationClient
{
	private readonly ClientWebSocket _client = new();
	private readonly Uri _uri;

	public SocketConfigurationClient(Uri host, string appId, string appSecret, string environment)
	{
		_uri = new Uri($"{host.AbsoluteUri}ws");
		_client.Options.SetRequestHeader("app-id", appId);
		_client.Options.SetRequestHeader("app-secret", appSecret);
		_client.Options.SetRequestHeader("app-env", environment);
	}

	public async Task GetConfigurationAsync(Action<byte[], int> dataAction, CancellationToken cancellationToken = default)
	{
		var attempts = 0;

		RUN:
		try
		{
			attempts++;

			await _client.ConnectAsync(_uri, cancellationToken);

			var buffer = new byte[1024 * 4];

			while (_client.State == WebSocketState.Open)
			{
				var result = await _client.ReceiveAsync(buffer, cancellationToken);
				if (result.MessageType == WebSocketMessageType.Close)
				{
					await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
				}
				else
				{
					dataAction(buffer, result.Count);
				}
			}
		}
		catch (Exception exception)
		{
			switch (exception)
			{
				case TimeoutException:
				case TaskCanceledException:
				case OperationCanceledException:
					if (attempts < 10)
					{
						await Task.Delay(2000, cancellationToken);
						goto RUN;
					}

					break;
			}

			throw;
		}
	}
}