﻿using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Claims;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public class AppInfoStatusBusiness : CommandObject<AppInfoStatusBusiness>, IDomainService
{
	[Inject]
	public IAppInfoRepository AppInfoRepository { get; set; }

	[Inject]
	public ITeamRepository TeamRepository { get; set; }

	[Inject]
	private UserPrincipal Identity { get; set; }

	[FactoryExecute]
	protected async Task ExecuteAsync(string id, AppStatus status, CancellationToken cancellationToken = default)
	{
		var aggregate = await AppInfoRepository.GetAsync(id, true, cancellationToken);
		if (aggregate == null)
		{
			throw new AppInfoNotFoundException(id);
		}

		var team = await TeamRepository.GetAsync(aggregate.TeamId, false, cancellationToken);
		if (team.OwnerId != Identity.UserId)
		{
			throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
		}

		switch (status)
		{
			case AppStatus.Disabled:
				aggregate.Disable();
				break;
			case AppStatus.Enabled:
				aggregate.Enable();
				break;
			case AppStatus.None:
			default:
				throw new ArgumentOutOfRangeException(Resources.IDS_ERROR_APPINFO_STATUS_INVALID);
		}

		await AppInfoRepository.UpdateAsync(aggregate, true, cancellationToken);
	}
}