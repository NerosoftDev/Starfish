using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface IAppsApi
{
	[Get("/api/apps")]
	Task<IApiResponse<List<AppInfoItemDto>>> QueryAsync([Query] AppInfoCriteria criteria, int skip = Constants.Query.Skip, int count = Constants.Query.Count, CancellationToken cancellationToken = default);

	[Get("/api/apps/count")]
	Task<IApiResponse<int>> CountAsync([Query] AppInfoCriteria criteria, CancellationToken cancellationToken = default);

	[Get("/api/apps/{id}")]
	Task<IApiResponse<AppInfoDetailDto>> GetAsync(long id, CancellationToken cancellationToken = default);

	[Post("/api/apps")]
	Task<IApiResponse> CreateAsync([Body] AppInfoCreateDto data, CancellationToken cancellationToken = default);

	[Put("/api/apps/{id}")]
	Task<IApiResponse> UpdateAsync(long id, [Body] AppInfoUpdateDto data, CancellationToken cancellationToken = default);

	[Put("/api/apps/{id}/secret")]
	Task<IApiResponse> SetSecretAsync(long id, [Body] AppInfoSetSecretDto data, CancellationToken cancellationToken = default);
	
	[Delete("/api/apps/{id}")]
	Task<IApiResponse> DeleteAsync(long id, CancellationToken cancellationToken = default);

	[Put("/api/apps/{id}/{status}")]
	Task<IApiResponse> ChangeStatusAsync(long id, string status, CancellationToken cancellationToken = default);
}