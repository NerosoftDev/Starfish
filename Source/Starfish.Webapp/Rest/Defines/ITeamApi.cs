using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface ITeamApi
{
	[Get("/api/team")]
	Task<IApiResponse<List<TeamItemDto>>> QueryAsync([Query] TeamCriteria criteria, int skip = Constants.Query.Skip, int count = Constants.Query.Count, CancellationToken cancellationToken = default);

	[Get("/api/team/count")]
	Task<IApiResponse<int>> CountAsync([Query] TeamCriteria criteria, CancellationToken cancellationToken = default);

	[Get("/api/team/{id}")]
	Task<IApiResponse<TeamDetailDto>> GetAsync(long id, CancellationToken cancellationToken = default);

	[Post("/api/team")]
	Task<IApiResponse> CreateAsync([Body] TeamEditDto data, CancellationToken cancellationToken = default);

	[Put("/api/team/{id}")]
	Task<IApiResponse> UpdateAsync(long id, [Body] TeamEditDto data, CancellationToken cancellationToken = default);

	[Get("/api/team/{id}/member")]
	Task<IApiResponse<List<TeamMemberDto>>> GetMembersAsync(long id, CancellationToken cancellationToken = default);

	[Post("/api/team/{id}/member")]
	Task<IApiResponse> AppendMemberAsync(long id, [Body] List<long> userIds, CancellationToken cancellationToken = default);

	[Delete("/api/team/{id}/member")]
	Task<IApiResponse> RemoveMemberAsync(long id, [Body] List<long> userIds, CancellationToken cancellationToken = default);

	[Delete("/api/team/{id}/quit")]
	Task<IApiResponse> QuitAsync(long id, CancellationToken cancellationToken = default);
}