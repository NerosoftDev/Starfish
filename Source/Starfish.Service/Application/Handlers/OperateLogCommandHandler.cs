using Microsoft.Extensions.Logging;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 操作日志命令处理器
/// </summary>
public class OperateLogCommandHandler : CommandHandlerBase,
                                        IHandler<OperateLogCreateCommand>
{
	private readonly IOperateLogRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="unitOfWork"></param>
	/// <param name="factory"></param>
	/// <param name="repository"></param>
	/// <param name="logger"></param>
	public OperateLogCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory, IOperateLogRepository repository, ILoggerFactory logger)
		: base(unitOfWork, factory, logger)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task HandleAsync(OperateLogCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var entity = OperateLog.Create(message.Module, message.Type, message.Description, message.UserName, message.OperateTime, message.Error, message.RequestTraceId);
			await _repository.InsertAsync(entity, true, cancellationToken);
		});
	}
}