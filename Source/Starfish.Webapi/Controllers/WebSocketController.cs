using System.Net.WebSockets;
using System.Threading.Channels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// WebSocket控制器
/// </summary>
[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class WebSocketController : ControllerBase
{
	private readonly ConnectionContainer _container;

	/// <summary>
	/// <see cref="EventStreamController"/>.ctor
	/// </summary>
	/// <param name="container"></param>
	public WebSocketController(ConnectionContainer container)
	{
		_container = container;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[Route("/ws")]
	public async Task HandleAsync(string id, string teamId, string name, string secret)
	{
		if (HttpContext.WebSockets.IsWebSocketRequest)
		{
			var configId = await AuthAsync(id, teamId, name, secret);

			using var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();

			var connection = _container.GetOrAdd(configId, HttpContext.Connection.Id);

			await Task.WhenAny(MonitorChannelAsync(connection.Channel, socket), MonitorClientAsync(socket));

			await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);

			_container.Remove(configId, HttpContext.Connection.Id);
		}
		else
		{
			HttpContext.Response.StatusCode = 400;
		}

		async Task MonitorChannelAsync(Channel<Tuple<string, string>> channel, WebSocket webSocket)
		{
			while (await channel.Reader.WaitToReadAsync(HttpContext.RequestAborted))
			{
				if (!channel.Reader.TryRead(out var item))
				{
					continue;
				}

				if (item.Item1 != "*" && item.Item1 != HttpContext.Connection.Id)
				{
					continue;
				}

				var bytes = Encoding.UTF8.GetBytes(item.Item2);
				await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, WebSocketMessageFlags.None, CancellationToken.None);
			}
		}

		async Task MonitorClientAsync(WebSocket webSocket)
		{
			while (webSocket.CloseStatus == null)
			{
				var buffer = new byte[1024 * 4];
				var received = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
				switch (received.MessageType)
				{
					case WebSocketMessageType.Close:
						await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
						break;
					case WebSocketMessageType.Text:
						var message = Encoding.UTF8.GetString(buffer, 0, received.Count);
						Console.WriteLine(message);
						break;
					case WebSocketMessageType.Binary:
						break;
				}
			}
		}
	}

	private Task<string> AuthAsync(string id, string teamId, string name, string secret)
	{
		// var app = HttpContext.Request.Headers[Constants.RequestHeaders.App];
		// var secret = HttpContext.Request.Headers[Constants.RequestHeaders.Secret];

		var service = HttpContext.RequestServices.GetRequiredService<IConfigurationApplicationService>();
		return service.AuthorizeAsync(id, teamId, name, secret, HttpContext.RequestAborted);
		
	}
}