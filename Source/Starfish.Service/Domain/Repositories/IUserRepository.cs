using System.Linq.Expressions;
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

	/// <summary>
	/// 检查邮箱是否存在
	/// </summary>
	/// <param name="email"></param>
	/// <param name="ignoreId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> CheckEmailExistsAsync(string email, int ignoreId, CancellationToken cancellationToken = default);

	/// <summary>
	/// 查询用户
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="builder"></param>
	/// <param name="page"></param>
	/// <param name="size"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<User>> FindAsync(Expression<Func<User, bool>> predicate, Func<IQueryable<User>, IQueryable<User>> builder, int page, int size, CancellationToken cancellationToken = default);

	Task<User> GetAsync(int id, bool tracking, Func<IQueryable<User>, IQueryable<User>> propertyAction, CancellationToken cancellationToken = default);
}