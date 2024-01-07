using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface ITeamApi
{
	[Get("/api/team")]
	Task<IApiResponse<List<TeamItemDto>>> QueryAsync([Query] TeamCriteria criteria, int page, int size, CancellationToken cancellationToken = default);

	[Get("/api/team/count")]
	Task<IApiResponse<int>> CountAsync([Query] TeamCriteria criteria, CancellationToken cancellationToken = default);

	[Get("/api/team/{id}")]
	Task<IApiResponse<TeamDetailDto>> GetAsync(int id, CancellationToken cancellationToken = default);

	[Post("/api/team")]
	Task<IApiResponse> CreateAsync([Body] TeamEditDto data, CancellationToken cancellationToken = default);

	[Put("/api/team/{id}")]
	Task<IApiResponse> UpdateAsync(int id, [Body] TeamEditDto data, CancellationToken cancellationToken = default);

	[Get("/api/team/{id}/member")]
	Task<ApiResponse<List<TeamMemberDto>>> GetMembersAsync(int id, CancellationToken cancellationToken = default);

	[Post("/api/team/{id}/member")]
	Task<IApiResponse> AppendMemberAsync(int id, [Body] List<int> userIds, CancellationToken cancellationToken = default);

	[Delete("/api/team/{id}/member")]
	Task<IApiResponse> RemoveMemberAsync(int id, [Body] List<int> userIds, CancellationToken cancellationToken = default);

	[Delete("/api/team/{id}/quit")]
	Task<IApiResponse> QuitAsync(int id, CancellationToken cancellationToken = default);
}