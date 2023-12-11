using IdentityModel;
using Microsoft.Extensions.Logging;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Token命令处理器
/// </summary>
public sealed class TokenCommandHandler : CommandHandlerBase,
                                          IHandler<TokenCreateCommand>,
                                          IHandler<TokenDeleteCommand>
{
	private readonly TokenRepository _repository;

	/// <summary>
	/// 初始化<see cref="TokenCommandHandler"/>.
	/// </summary>
	/// <param name="repository">仓库对象</param>
	/// <param name="unitOfWork">工作单元管理器</param>
	/// <param name="factory">业务对象工厂</param>
	/// <param name="logger">日志工厂</param>
	public TokenCommandHandler(TokenRepository repository, IUnitOfWorkManager unitOfWork, IObjectFactory factory, ILoggerFactory logger)
		: base(unitOfWork, factory, logger)
	{
		_repository = repository;
	}

	/// <inheritdoc/>
	public Task HandleAsync(TokenCreateCommand message, MessageContext messageContext, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var entity = Token.Create(message.Type, message.Token, message.Subject, message.Issues, message.Expires);
			await _repository.InsertAsync(entity, true, cancellationToken);
		});
	}

	/// <inheritdoc/>
	public Task HandleAsync(TokenDeleteCommand message, MessageContext messageContext, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var key = message.Token.ToSha256();
			var entity = await _repository.GetAsync(t => t.Type == message.Type && t.Key == key, true, cancellationToken);
			await _repository.DeleteAsync(entity, true, cancellationToken);
		});
	}
}