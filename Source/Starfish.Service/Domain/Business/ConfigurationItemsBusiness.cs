using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public class ConfigurationItemsBusiness : CommandObjectBase<ConfigurationItemsBusiness>, IDomainService
{
	[Inject]
	public ITeamRepository TeamRepository { get; set; }

	[Inject]
	public IConfigurationRepository ConfigurationRepository { get; set; }

	[FactoryExecute]
	protected async Task ExecuteAsync(string id, IDictionary<string, string> items, CancellationToken cancellationToken = default)
	{
		var aggregate = await ConfigurationRepository.GetAsync(id, true, cancellationToken);

		if (aggregate == null)
		{
			throw new ConfigurationNotFoundException(id);
		}

		var permission = await TeamRepository.CheckPermissionAsync(id, Identity.UserId, cancellationToken);

		switch (permission)
		{
			case PermissionState.None: // 无权限
				throw new ConfigurationNotFoundException(id);
			case PermissionState.Edit:
				break;
			case PermissionState.Read:
				throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
			default:
				throw new ArgumentOutOfRangeException();
		}

		aggregate.UpdateItem(items);

		await ConfigurationRepository.UpdateAsync(aggregate, true, cancellationToken);
	}

	[FactoryExecute]
	protected async Task ExecuteAsync(string id, string key, string value, CancellationToken cancellationToken = default)
	{
		var aggregate = await ConfigurationRepository.GetAsync(id, true, cancellationToken);

		if (aggregate == null)
		{
			throw new ConfigurationNotFoundException(id);
		}

		var permission = await TeamRepository.CheckPermissionAsync(id, Identity.UserId, cancellationToken);

		switch (permission)
		{
			case PermissionState.None: // 无权限
				throw new ConfigurationNotFoundException(id);
			case PermissionState.Edit:
				break;
			case PermissionState.Read:
				throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
			default:
				throw new ArgumentOutOfRangeException();
		}

		aggregate.UpdateItem(key, value);

		await ConfigurationRepository.UpdateAsync(aggregate, true, cancellationToken);
	}
}