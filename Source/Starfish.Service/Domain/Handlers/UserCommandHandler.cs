using Microsoft.Extensions.Logging;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户命令处理器
/// </summary>
public sealed class UserCommandHandler : CommandHandlerBase, IHandler<UserCreateCommand>
{
	public UserCommandHandler(IUnitOfWorkManager unitOfWork, ILoggerFactory logger)
		: base(unitOfWork, logger)
	{
	}

	public UserCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory, ILoggerFactory logger)
		: base(unitOfWork, factory, logger)
	{
	}

	/// <inheritdoc/>
	public Task HandleAsync(UserCreateCommand message, MessageContext messageContext, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}