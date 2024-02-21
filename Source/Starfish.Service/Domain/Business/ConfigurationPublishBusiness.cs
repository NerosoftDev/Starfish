using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Claims;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;

// ReSharper disable UnusedMember.Global

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用配置发布领域服务
/// </summary>
public class ConfigurationPublishBusiness : CommandObjectBase<ConfigurationPublishBusiness>, IDomainService
{
	[Inject]
	public ITeamRepository TeamRepository { get; set; }

	[Inject]
	public IConfigurationRepository ConfigurationRepository { get; set; }

	[FactoryExecute]
	protected async Task ExecuteAsync(string id, string version, string comment, CancellationToken cancellationToken = default)
	{
		string[] includeProperties = [nameof(Configuration.Items), nameof(Configuration.Revisions), nameof(Configuration.Archive)];

		var aggregate = await ConfigurationRepository.GetAsync(id, true, includeProperties, cancellationToken);

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

		aggregate.Publish(version, comment, Identity.Username);

		await ConfigurationRepository.UpdateAsync(aggregate, true, cancellationToken);
	}
}