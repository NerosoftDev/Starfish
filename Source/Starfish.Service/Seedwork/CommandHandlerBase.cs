using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Service;

/// <summary>
/// CommandHandler基类
/// </summary>
public abstract class CommandHandlerBase
{
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
	protected CommandHandlerBase(IUnitOfWorkManager unitOfWork)
	{
		UnitOfWork = unitOfWork;
	}

	/// <summary>
	/// 初始化<see cref="CommandHandlerBase"/>.
	/// </summary>
	/// <param name="unitOfWork"></param>
	/// <param name="factory"></param>
	protected CommandHandlerBase(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
		: this(unitOfWork)
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
			response.Failure(exception);
		}

		return response;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	protected virtual async Task ExecuteAsync([NotNull] Func<Task> action)
	{
		using var uow = UnitOfWork.Begin();
		await action();
		await uow.CommitAsync();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="action"></param>
	/// <param name="next"></param>
	/// <returns></returns>
	protected virtual async Task ExecuteAsync<TResult>([NotNull] Func<Task<TResult>> action, Action<TResult> next)
	{
		using var uow = UnitOfWork.Begin();
		var result = await action();
		await uow.CommitAsync();
		next(result);
	}
}