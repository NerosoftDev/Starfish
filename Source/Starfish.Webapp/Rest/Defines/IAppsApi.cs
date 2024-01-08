using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface IAppsApi
{
	[Get("/api/apps")]
	Task<IApiResponse<List<AppInfoItemDto>>> QueryAsync([Query] AppInfoCriteria criteria, int page, int size, CancellationToken cancellationToken = default);
}