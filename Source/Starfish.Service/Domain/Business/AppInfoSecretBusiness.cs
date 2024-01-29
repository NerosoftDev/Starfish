using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Claims;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public class AppInfoSecretBusiness : CommandObject<AppInfoSecretBusiness>, IDomainService
{
	[Inject]
	public IAppInfoRepository AppsRepository { get; set; }

	[Inject]
	public ITeamRepository TeamRepository { get; set; }

	[Inject]
	private UserPrincipal Identity { get; set; }

	[FactoryExecute]
	protected async Task ExecuteAsync(string id, string secret, CancellationToken cancellationToken = default)
	{
		var aggregate = await AppsRepository.GetAsync(id, true, cancellationToken);
		if (aggregate == null)
		{
			throw new AppInfoNotFoundException(id);
		}

		var team = await TeamRepository.GetAsync(aggregate.TeamId, false, cancellationToken);
		if (team.OwnerId != Identity.UserId)
		{
			throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
		}

		aggregate.SetSecret(secret);

		await AppsRepository.UpdateAsync(aggregate, true, cancellationToken);
	}
}