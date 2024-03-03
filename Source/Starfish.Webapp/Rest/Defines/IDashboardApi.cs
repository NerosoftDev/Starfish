using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

public interface IDashboardApi
{
	[Get("/api/dashboard/connections/count")]
	Task<IApiResponse<int>> GetConnectionCountAsync(CancellationToken cancellationToken = default);

	[Get("/api/dashboard/connections")]
	Task<IApiResponse<List<ConnectionInfoDto>>> GetConnectionsListAsync(CancellationToken cancellationToken = default);

	[Get("/api/dashboard/configurations/count")]
	Task<IApiResponse<int>> GetConfigurationCountAsync(CancellationToken cancellationToken = default);

	[Get("/api/dashboard/configurations/items/count")]
	Task<IApiResponse<int>> GetConfigurationItemCountAsync(CancellationToken cancellationToken = default);

	[Get("/api/dashboard/teams/count")]
	Task<IApiResponse<int>> GetTeamCountAsync(CancellationToken cancellationToken = default);
}