using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Claims;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public class AppInfoSecretBusiness : CommandObject<AppInfoSecretBusiness>, IDomainService
{
	[Inject]
	public IAppInfoRepository AppInfoRepository { get; set; }

	[Inject]
	public ITeamRepository TeamRepository { get; set; }

	[Inject]
	private UserPrincipal Identity { get; set; }

	[FactoryExecute]
	protected async Task ExecuteAsync(long id, string secret, CancellationToken cancellationToken = default)
	{
		var aggregate = await AppInfoRepository.GetAsync(id, true, cancellationToken);
		if (aggregate == null)
		{
			throw new AppInfoNotFoundException(id);
		}

		var team = await TeamRepository.GetAsync(aggregate.TeamId, false, cancellationToken);
		if (team.OwnerId != Identity.GetUserIdOfInt32())
		{
			throw new UnauthorizedAccessException(Resources.IDS_ERROR_TEAM_ONLY_ALLOW_OWNER_CHANGE_MEMBER);
		}

		aggregate.SetSecret(secret);

		await AppInfoRepository.UpdateAsync(aggregate, true, cancellationToken);
	}
}