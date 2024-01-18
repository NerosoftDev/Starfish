﻿using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public interface ITeamRepository : IBaseRepository<DataContext, Team, int>
{
	/// <summary>
	/// 查询指定用户所属团队
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<Team>> GetTeamsOfUserAsync(int userId, CancellationToken cancellationToken = default);

	Task<List<TeamMember>> GetMembersAsync(int id, CancellationToken cancellationToken = default);
}