using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户仓储接口
/// </summary>
public interface IUserRepository : IBaseRepository<DataContext, User, int>
{
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
	/// 检查手机号是否存在
	/// </summary>
	/// <param name="phone"></param>
	/// <param name="ignoreId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> CheckPhoneExistsAsync(string phone, int ignoreId, CancellationToken cancellationToken = default);
}