using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

public class TeamRepository : BaseRepository<DataContext, Team, string>, ITeamRepository
{
	public TeamRepository(IContextProvider provider)
		: base(provider)
	{
	}

	public Task<List<Team>> GetTeamsOfUserAsync(string userId, CancellationToken cancellationToken = default)
	{
		var memberSet = Context.Set<TeamMember>();
		var teamSet = Context.Set<Team>();
		var query = from team in teamSet
		            join member in memberSet on team.Id equals member.TeamId
		            where member.UserId == userId
		            select team;
		return query.ToListAsync(cancellationToken);
	}

	public Task<List<TeamMember>> GetMembersAsync(string id, CancellationToken cancellationToken = default)
	{
		var query = Context.Set<TeamMember>()
		                   .Include(t => t.User)
		                   .Where(x => x.TeamId == id);
		return query.ToListAsync(cancellationToken);
	}

	public async Task<int> CheckPermissionAsync(string id, string userId, CancellationToken cancellationToken = default)
	{
		var query = from team in Context.Set<Team>()
		            join member in Context.Set<TeamMember>() on team.Id equals member.TeamId
		            where team.Id == id
		            select new
		            {
			            member.UserId,
			            IsOwner = member.UserId == team.OwnerId
		            };

		var result = await query.ToListAsync(cancellationToken);

		var user = result.FirstOrDefault(x => x.UserId == userId);
		if (user == null)
		{
			return 0;
		}

		return user.IsOwner ? 1 : 2;
	}
}