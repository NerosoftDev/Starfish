using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public class ConfigurationStatusBusiness : CommandObjectBase<ConfigurationStatusBusiness>, IDomainService
{
	[Inject]
	public ITeamRepository TeamRepository { get; set; }

	[Inject]
	public IConfigurationRepository ConfigurationRepository { get; set; }

	[FactoryExecute]
	protected async Task ExecuteAsync(string id, bool availability, CancellationToken cancellationToken = default)
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

		if (availability)
		{
			aggregate.Enable();
		}
		else
		{
			aggregate.Disable();
		}

		await ConfigurationRepository.UpdateAsync(aggregate, true, cancellationToken);
	}
}