using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

public interface ITeamApplicationService : IApplicationService
{
	Task<List<TeamItemDto>> QueryAsync(TeamCriteria criteria, int skip, int count, CancellationToken cancellationToken = default);

	Task<int> CountAsync(TeamCriteria criteria, CancellationToken cancellationToken = default);

	Task<TeamDetailDto> GetAsync(int id, CancellationToken cancellationToken = default);
	
	Task<int> CreateAsync(TeamEditDto data, CancellationToken cancellationToken = default);

	Task UpdateAsync(int id, TeamEditDto data, CancellationToken cancellationToken = default);

	Task<List<TeamMemberDto>> QueryMembersAsync(int id, CancellationToken cancellationToken = default);

	Task AppendMembersAsync(int id, List<int> userIds, CancellationToken cancellationToken = default);

	Task RemoveMembersAsync(int id, List<int> userIds, CancellationToken cancellationToken = default);

	Task QuitAsync(int id, CancellationToken cancellationToken = default);
}