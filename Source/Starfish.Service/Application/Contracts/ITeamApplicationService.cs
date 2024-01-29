using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

public interface ITeamApplicationService : IApplicationService
{
	Task<List<TeamItemDto>> QueryAsync(TeamCriteria criteria, int skip, int count, CancellationToken cancellationToken = default);

	Task<int> CountAsync(TeamCriteria criteria, CancellationToken cancellationToken = default);

	Task<TeamDetailDto> GetAsync(string id, CancellationToken cancellationToken = default);
	
	Task<string> CreateAsync(TeamEditDto data, CancellationToken cancellationToken = default);

	Task UpdateAsync(string id, TeamEditDto data, CancellationToken cancellationToken = default);

	Task<List<TeamMemberDto>> QueryMembersAsync(string id, CancellationToken cancellationToken = default);

	Task AppendMembersAsync(string id, List<string> userIds, CancellationToken cancellationToken = default);

	Task RemoveMembersAsync(string id, List<string> userIds, CancellationToken cancellationToken = default);

	Task QuitAsync(string id, CancellationToken cancellationToken = default);
}