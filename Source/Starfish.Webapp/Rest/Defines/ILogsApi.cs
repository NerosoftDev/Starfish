using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface ILogsApi
{
	[Get("/api/logs")]
	Task<IApiResponse<List<OperateLogDto>>> QueryAsync([Query] OperateLogCriteria criteria, int skip = Constants.Query.Skip, int count = Constants.Query.Count, CancellationToken cancellationToken = default);

	[Get("/api/logs/count")]
	Task<IApiResponse<int>> CountAsync([Query] OperateLogCriteria criteria, CancellationToken cancellationToken = default);
}