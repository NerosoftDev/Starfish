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
	public async Task HandleAsync(string team, string app, string secret, string env)
	{
		var appId = await AuthAsync(team, app, secret);
		//var environment = HttpContext.Request.Headers[Constants.RequestHeaders.Env];
		Response.Headers.Append(HeaderNames.ContentType, "text/event-stream");
		Response.Headers.Append(HeaderNames.Connection, "close");
		try
		{
			var connection = _container.GetOrAdd(appId, env, HttpContext.Connection.Id);

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
			_container.Remove(appId, env, HttpContext.Connection.Id);
		}
	}

	private Task<long> AuthAsync(string team, string app, string secret)
	{
		// var app = HttpContext.Request.Headers[Constants.RequestHeaders.App];
		// var secret = HttpContext.Request.Headers[Constants.RequestHeaders.Secret];

		var service = HttpContext.RequestServices.GetRequiredService<IAppsApplicationService>();
		return service.AuthorizeAsync(app, secret, HttpContext.RequestAborted);
	}
}