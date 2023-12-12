using Microsoft.Extensions.Logging;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用信息命令处理器
/// </summary>
public class AppInfoCommandHandler : CommandHandlerBase,
									 IHandler<AppInfoCreateCommand>,
									 IHandler<AppInfoUpdateCommand>,
									 IHandler<AppInfoDeleteCommand>,
									 IHandler<ChangeAppInfoStatusCommand>,
									 IHandler<ResetAppInfoSecretCommand>
{
	private readonly IAppInfoRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="unitOfWork"></param>
	/// <param name="factory"></param>
	/// <param name="repository"></param>
	/// <param name="logger"></param>
	public AppInfoCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory, IAppInfoRepository repository, ILoggerFactory logger)
		: base(unitOfWork, factory, logger)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task HandleAsync(AppInfoCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await CheckCodeAsync(message.Item1.Code);
			var entity = AppInfo.Create(message.Item1.Name, message.Item1.Code, message.Item1.Description);
			await _repository.InsertAsync(entity, true, cancellationToken);
			return entity.Id;
		}, context.Response);

		async Task CheckCodeAsync(string code)
		{
			var exists = await _repository.GetByCodeAsync(code, cancellationToken);
			if (exists != null)
			{
				throw new ConflictException(Resources.IDS_ERROR_APPINFO_CODE_UNAVAILABLE);
			}
		}
	}

	/// <inheritdoc />
	public Task HandleAsync(AppInfoUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var entity = await _repository.GetAsync(message.Item1, cancellationToken);
			if (entity == null)
			{
				throw new AppInfoNotFoundException(message.Item1);
			}

			entity.Update(message.Item2.Name, message.Item2.Description);
			switch (message.Item2.IsEnabled)
			{
				case true:
					entity.Enable();
					break;
				case false:
					entity.Disable();
					break;
			}

			await _repository.UpdateAsync(entity, true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(AppInfoDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var entity = await _repository.GetAsync(message.Item1, cancellationToken);
			if (entity == null)
			{
				throw new AppInfoNotFoundException(message.Item1);
			}

			entity.RaiseEvent(new AppInfoDeletedEvent());
			await _repository.DeleteAsync(entity, true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ChangeAppInfoStatusCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var entity = await _repository.GetAsync(message.Item1, cancellationToken);
			if (entity == null)
			{
				throw new AppInfoNotFoundException(message.Item1);
			}

			switch (message.Item2)
			{
				case AppStatus.Disabled:
					entity.Disable();
					break;
				case AppStatus.Enabled:
					entity.Enable();
					break;
				case AppStatus.None:
				default:
					throw new InvalidAppInfoStatusException(Resources.IDS_ERROR_APPINFO_STATUS_INVALID);
			}

			await _repository.UpdateAsync(entity, true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(ResetAppInfoSecretCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var entity = await _repository.GetAsync(message.Item1, cancellationToken);
			if (entity == null)
			{
				throw new AppInfoNotFoundException(message.Item1);
			}

			entity.ResetSecret();
			await _repository.UpdateAsync(entity, true, cancellationToken);
		});
	}
}