using System.Net.WebSockets;

namespace Nerosoft.Starfish.Client;

internal class SocketConfigurationClient : IConfigurationClient
{
	private readonly ClientWebSocket _client = new();
	private readonly Uri _uri;

	public SocketConfigurationClient(Uri host, string appId, string appSecret, string environment)
	{
		_uri = new Uri($"{host.AbsoluteUri}ws");
		_client.Options.SetRequestHeader(Constants.RequestHeaders.AppId, appId);
		_client.Options.SetRequestHeader(Constants.RequestHeaders.AppSecret, appSecret);
		_client.Options.SetRequestHeader(Constants.RequestHeaders.AppEnv, environment);
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

#if __MOBILE__
					await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
#endif
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