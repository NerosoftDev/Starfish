using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public interface ITeamRepository : IBaseRepository<DataContext, Team, string>
{
	/// <summary>
	/// 查询指定用户所属团队
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<Team>> GetTeamsOfUserAsync(string userId, CancellationToken cancellationToken = default);

	Task<List<TeamMember>> GetMembersAsync(string id, CancellationToken cancellationToken = default);
	
	Task<PermissionState> CheckPermissionAsync(string id, string userId, CancellationToken cancellationToken = default);
}