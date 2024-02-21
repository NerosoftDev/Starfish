using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface IAdministratorApi
{
	[Get("/api/administrator")]
	Task<IApiResponse<List<AdministratorItemDto>>> QueryAsync([Query] AdministratorCriteria criteria, int skip = Constants.Query.Skip, int count = Constants.Query.Count, CancellationToken cancellationToken = default);

	[Post("/api/administrator")]
	Task<IApiResponse> AssignAsync([Body] AdministratorAssignDto data, CancellationToken cancellationToken = default);

	[Get("/api/administrator/count")]
	Task<IApiResponse<int>> CountAsync([Query] AdministratorCriteria criteria, CancellationToken cancellationToken = default);

	[Delete("/api/administrator/{userId}")]
	Task<IApiResponse> DeleteAsync(string userId, CancellationToken cancellationToken = default);
}