using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户相关应用事件处理器
/// </summary>
public class UserEventSubscriber : IHandler<UserAuthSucceedEvent>,
                                   IHandler<UserAuthFailedEvent>
{
	/// <summary>
	/// 处理用户认证成功事件
	/// </summary>
	/// <param name="message"></param>
	/// <param name="messageContext"></param>
	/// <param name="cancellationToken"></param>
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
	public async Task HandleAsync(UserAuthFailedEvent message, MessageContext messageContext, CancellationToken cancellationToken = default)
	{
		await Task.CompletedTask;
	}
}