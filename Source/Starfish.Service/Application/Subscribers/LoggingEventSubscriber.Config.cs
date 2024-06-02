using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;
using Newtonsoft.Json;

namespace Nerosoft.Starfish.Application;

internal partial class LoggingEventSubscriber
{
	private const string MODULE_CONFIG = "config";

	/// <summary>
	/// 处理配置启用事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationEnabledEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Configuration>();
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "status.enable",
			Content = GenerateLogContent(aggregate.Id, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理配置禁用事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationDisableEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Configuration>();
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "status.disable",
			Content = GenerateLogContent(aggregate.Id, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理配置密钥重置事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationSecretChangedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Configuration>();
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "secret",
			Content = GenerateLogContent(aggregate.Id, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理配置更新事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationUpdatedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Configuration>();
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "update",
			Content = GenerateLogContent(aggregate.Id, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};

		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理配置节点创建事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationCreatedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "create",
			Content = GenerateLogContent(@event.Configuration.Id, @event.Configuration.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理配置节点删除事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationDeletedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Configuration>();

		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "delete",
			Content = GenerateLogContent(aggregate.Id, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	/// <summary>
	/// 处理配置发布事件
	/// </summary>
	/// <param name="event"></param>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Subscribe]
	public Task HandleAsync(ConfigurationPublishedEvent @event, MessageContext context, CancellationToken cancellationToken = default)
	{
		var aggregate = @event.GetAggregate<Configuration>();

		var command = new OperateLogCreateCommand
		{
			Module = MODULE_CONFIG,
			Type = "publish",
			Content = GenerateLogContent(aggregate.Id, aggregate.Name),
			OperateTime = DateTime.Now,
			RequestTraceId = context.RequestTraceId,
			UserName = context.User?.Identity?.Name
		};
		return _bus.SendAsync(command, new SendOptions { RequestTraceId = context.RequestTraceId }, null, cancellationToken);
	}

	private static string GenerateLogContent(params object[] args)
	{
		if (args == null || args.Length == 0)
		{
			return string.Empty;
		}

		var items = args.Select(t => t.ToString());

		return JsonConvert.SerializeObject(items);
	}
}