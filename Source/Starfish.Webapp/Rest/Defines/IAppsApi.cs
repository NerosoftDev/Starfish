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
	Task<IApiResponse<AppInfoDetailDto>> GetAsync(string id, CancellationToken cancellationToken = default);

	[Post("/api/apps")]
	Task<IApiResponse> CreateAsync([Body] AppInfoCreateDto data, CancellationToken cancellationToken = default);

	[Put("/api/apps/{id}")]
	Task<IApiResponse> UpdateAsync(string id, [Body] AppInfoUpdateDto data, CancellationToken cancellationToken = default);

	[Put("/api/apps/{id}/secret")]
	Task<IApiResponse> SetSecretAsync(string id, [Body] AppInfoSetSecretDto data, CancellationToken cancellationToken = default);
	
	[Delete("/api/apps/{id}")]
	Task<IApiResponse> DeleteAsync(string id, CancellationToken cancellationToken = default);

	[Put("/api/apps/{id}/{status}")]
	Task<IApiResponse> ChangeStatusAsync(string id, string status, CancellationToken cancellationToken = default);
}