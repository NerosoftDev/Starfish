using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

public interface ITeamApplicationService : IApplicationService
{
	Task<List<TeamItemDto>> QueryAsync(TeamCriteria criteria, int skip, int count, CancellationToken cancellationToken = default);

	Task<int> CountAsync(TeamCriteria criteria, CancellationToken cancellationToken = default);

	Task<TeamDetailDto> GetAsync(long id, CancellationToken cancellationToken = default);
	
	Task<long> CreateAsync(TeamEditDto data, CancellationToken cancellationToken = default);

	Task UpdateAsync(long id, TeamEditDto data, CancellationToken cancellationToken = default);

	Task<List<TeamMemberDto>> QueryMembersAsync(long id, CancellationToken cancellationToken = default);

	Task AppendMembersAsync(long id, List<long> userIds, CancellationToken cancellationToken = default);

	Task RemoveMembersAsync(long id, List<long> userIds, CancellationToken cancellationToken = default);

	Task QuitAsync(long id, CancellationToken cancellationToken = default);
}