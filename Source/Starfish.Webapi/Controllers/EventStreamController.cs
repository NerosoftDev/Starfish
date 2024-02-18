using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 事件流控制器
/// </summary>
[ApiController, ApiExplorerSettings(IgnoreApi = true)]
[AllowAnonymous]
public class EventStreamController : ControllerBase
{
	private readonly ConnectionContainer _container;
	private readonly ILogger<EventStreamController> _logger;

	/// <summary>
	/// <see cref="EventStreamController"/>.ctor
	/// </summary>
	/// <param name="container"></param>
	/// <param name="logger"></param>
	public EventStreamController(ConnectionContainer container, ILoggerFactory logger)
	{
		_container = container;
		_logger = logger.CreateLogger<EventStreamController>();
	}

	/// <summary>
	/// 处理请求
	/// </summary>
	/// <returns></returns>
	[Route("/es")]
	public async Task HandleAsync(string id, string teamId, string name, string secret)
	{
		var configId = await AuthAsync(id, teamId, name, secret);
		Response.Headers.Append(HeaderNames.ContentType, "text/event-stream");
		Response.Headers.Append(HeaderNames.Connection, "close");
		try
		{
			var connection = _container.GetOrAdd(configId, HttpContext.Connection.Id);

			while (await connection.Channel.Reader.WaitToReadAsync(HttpContext.RequestAborted))
			{
				if (!connection.Channel.Reader.TryRead(out var item))
				{
					continue;
				}

				if (item.Item1 != "*" && item.Item1 != HttpContext.Connection.Id)
				{
					continue;
				}

				HttpContext.RequestAborted.ThrowIfCancellationRequested();
				await HttpContext.Response.WriteAsync(item.Item2, Encoding.UTF8);
				await HttpContext.Response.Body.FlushAsync();
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "EventStream error");
		}
		finally
		{
			Response.Body.Close();
			_container.Remove(configId, HttpContext.Connection.Id);
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