using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Claims;
using Nerosoft.Euonia.Domain;

// ReSharper disable UnusedMember.Global

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用配置发布领域服务
/// </summary>
public class ConfigurationPublishBusiness : CommandObject<ConfigurationPublishBusiness>, IDomainService
{
	[Inject]
	public IAppInfoRepository AppInfoRepository { get; set; }

	[Inject]
	public IConfigurationRepository ConfigurationRepository { get; set; }

	[Inject]
	public UserPrincipal Identity { get; set; }

	[FactoryExecute]
	protected async Task ExecuteAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var permission = await AppInfoRepository.CheckPermissionAsync(appId, Identity.GetUserIdOfInt64(), cancellationToken);

		switch(permission)
		{
			case 0:
				throw new UnauthorizedAccessException();
			case 1:
				break;
			case 2:
				throw new UnauthorizedAccessException();
			default:
				throw new ArgumentOutOfRangeException();
		}

		var aggregate = await ConfigurationRepository.GetAsync(appId, environment, true, [], cancellationToken);

		if (aggregate == null)
		{
			throw new ConfigurationNotFoundException(appId, environment);
		}

		if (aggregate.Status == ConfigurationStatus.Disabled)
		{
			throw new ConfigurationDisabledException(appId, environment);
		}

		aggregate.SetStatus(ConfigurationStatus.Published);

		await ConfigurationRepository.UpdateAsync(aggregate, true, cancellationToken);
	}
}