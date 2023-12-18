using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户仓储接口
/// </summary>
public interface IUserRepository : IRepository<User, int>
{
	/// <summary>
	/// 根据用户ID查询用户
	/// </summary>
	/// <param name="id"></param>
	/// <param name="tracking"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<User> GetAsync(int id, bool tracking, CancellationToken cancellationToken = default);

	/// <summary>
	/// 根据用户名查询用户
	/// </summary>
	/// <param name="userName"></param>
	/// <param name="tracking"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<User> FindByUserNameAsync(string userName, bool tracking, CancellationToken cancellationToken = default);
	
	/// <summary>
	/// 检查用户名是否存在
	/// </summary>
	/// <param name="userName"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> CheckUserNameExistsAsync(string userName, CancellationToken cancellationToken = default);
}