using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 令牌事件订阅者
/// </summary>
public sealed class TokenEventSubscriber : IHandler<UserAuthSucceedEvent>,
                                           IHandler<TokenRefreshedEvent>
{
	private readonly IBus _bus;

	/// <summary>
	/// 初始化<see cref="TokenEventSubscriber"/>实例。
	/// </summary>
	/// <param name="bus"></param>
	public TokenEventSubscriber(IBus bus)
	{
		_bus = bus;
	}

	/// <inheritdoc/>
	public Task HandleAsync(UserAuthSucceedEvent message, MessageContext messageContext, CancellationToken cancellationToken = default)
	{
		var command = new TokenCreateCommand
		{
			Type = "refresh_token",
			Token = message.RefreshToken,
			Issues = message.TokenIssueTime,
			Subject = message.UserId.ToString(),
			Expires = message.TokenIssueTime.AddDays(30)
		};
		return _bus.SendAsync(command, cancellationToken);
	}

	/// <inheritdoc />
	public Task HandleAsync(TokenRefreshedEvent message, MessageContext messageContext, CancellationToken cancellationToken = new CancellationToken())
	{
		var command = new TokenDeleteCommand
		{
			Type = "refresh_token",
			Token = message.OriginToken
		};

		return _bus.SendAsync(command, cancellationToken);
	}
}