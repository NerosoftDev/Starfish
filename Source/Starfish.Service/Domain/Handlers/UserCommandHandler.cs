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
	/// <summary>
	/// 初始化<see cref="UserCommandHandler"/>.
	/// </summary>
	/// <param name="unitOfWork"></param>
	/// <param name="factory"></param>
	/// <param name="logger"></param>
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