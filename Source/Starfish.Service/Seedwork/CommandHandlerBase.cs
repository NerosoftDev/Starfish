using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Service;

/// <summary>
/// CommandHandler基类
/// </summary>
public abstract class CommandHandlerBase
{
	private readonly ILogger<CommandHandlerBase> _logger;

	/// <summary>
	/// 工作单元管理器
	/// </summary>
	protected virtual IUnitOfWorkManager UnitOfWork { get; }

	/// <summary>
	/// 业务对象工厂
	/// </summary>
	protected virtual IObjectFactory Factory { get; }

	/// <summary>
	/// 初始化<see cref="CommandHandlerBase"/>.
	/// </summary>
	/// <param name="unitOfWork"></param>
	/// <param name="logger"></param>
	protected CommandHandlerBase(IUnitOfWorkManager unitOfWork, ILoggerFactory logger)
	{
		UnitOfWork = unitOfWork;
		_logger = logger.CreateLogger<CommandHandlerBase>();
	}

	/// <summary>
	/// 初始化<see cref="CommandHandlerBase"/>.
	/// </summary>
	/// <param name="unitOfWork"></param>
	/// <param name="factory"></param>
	/// <param name="logger"></param>
	protected CommandHandlerBase(IUnitOfWorkManager unitOfWork, IObjectFactory factory, ILoggerFactory logger)
		: this(unitOfWork, logger)
	{
		Factory = factory;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="messageId"></param>
	/// <param name="action"></param>
	/// <returns></returns>
	protected virtual async Task<CommandResponse> ExecuteAsync(string messageId, [NotNull] Func<Task> action)
	{
		var response = new CommandResponse(messageId);
		try
		{
			using (var uow = UnitOfWork.Begin())
			{
				await action();
				await uow.CommitAsync();
			}

			response.Success();
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Command execute failed: {Message}", exception.Message);

			response.Failure(exception);
		}

		return response;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="messageId"></param>
	/// <param name="action"></param>
	/// <returns></returns>
	protected virtual async Task<CommandResponse<TResult>> ExecuteAsync<TResult>(string messageId, [NotNull] Func<Task<TResult>> action)
	{
		var response = new CommandResponse<TResult>(messageId);
		try
		{
			TResult result;
			using (var uow = UnitOfWork.Begin())
			{
				result = await action();
				await uow.CommitAsync();
			}

			response.Success(result);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Command execute failed: {Message}", exception.Message);
			response.Failure(exception);
		}

		return response;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="messageId"></param>
	/// <param name="action"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	protected virtual async Task ExecuteAsync(string messageId, [NotNull] Func<Task> action, [NotNull] MessageContext context)
	{
		var result = await ExecuteAsync(messageId, action);
		context.Response(result);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="messageId"></param>
	/// <param name="action"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	protected virtual async Task ExecuteAsync<TResult>(string messageId, [NotNull] Func<Task<TResult>> action, [NotNull] MessageContext context)
	{
		var result = await ExecuteAsync(messageId, action);
		context.Response(result);
	}
}