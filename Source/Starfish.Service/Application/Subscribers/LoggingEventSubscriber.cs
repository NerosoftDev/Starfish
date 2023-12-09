using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用日志收集处理器
/// </summary>
public sealed class LoggingEventSubscriber : IHandler<UserAuthSucceedEvent>, IHandler<UserAuthFailedEvent>
{
	/// <inheritdoc />
	public bool CanHandle(Type messageType)
	{
		return true;
	}

	/// <summary>
	/// 处理用户认证成功事件
	/// </summary>
	/// <param name="message"></param>
	/// <param name="messageContext"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="NotImplementedException"></exception>
	public async Task HandleAsync(UserAuthSucceedEvent message, MessageContext messageContext, CancellationToken cancellationToken = default)
	{
		await Task.CompletedTask;
	}

	/// <summary>
	/// 处理用户认证失败事件
	/// </summary>
	/// <param name="message"></param>
	/// <param name="messageContext"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="NotImplementedException"></exception>
	public async Task HandleAsync(UserAuthFailedEvent message, MessageContext messageContext, CancellationToken cancellationToken = default)
	{
		await Task.CompletedTask;
	}
}