using System.Linq.Expressions;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Domain;

public interface ITeamRepository : IRepository<Team, int>
{
	/// <summary>
	/// 根据ID查询团队信息
	/// </summary>
	/// <param name="id"></param>
	/// <param name="tracking"></param>
	/// <param name="properties"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<Team> GetAsync(int id, bool tracking, string[] properties, CancellationToken cancellationToken = default);

	/// <summary>
	/// 查询指定用户所属团队
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<Team>> GetTeamsOfUserAsync(int userId, CancellationToken cancellationToken = default);

	Task<List<Team>> FindAsync(Expression<Func<Team, bool>> predicate, Func<IQueryable<Team>, IQueryable<Team>> builder, int page, int size, CancellationToken cancellationToken = default);

	Task<List<TeamMember>> GetMembersAsync(int id, CancellationToken cancellationToken = default);
}