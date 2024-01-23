using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户应用服务接口
/// </summary>
public interface IUserApplicationService : IApplicationService
{
	/// <summary>
	/// 新增用户
	/// </summary>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<long> CreateAsync(UserCreateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 编辑用户
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateAsync(long id, UserUpdateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 查询符合条件的用户列表
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="count"></param>
	/// <param name="cancellationToken"></param>
	/// <param name="skip"></param>
	/// <returns></returns>
	Task<List<UserItemDto>> QueryAsync(UserCriteria criteria, int skip, int count, CancellationToken cancellationToken = default);

	/// <summary>
	/// 查询符合条件的用户数量
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<int> CountAsync(UserCriteria criteria, CancellationToken cancellationToken = default);

	/// <summary>
	/// 根据Id获取用户详情
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<UserDetailDto> GetAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 删除用户
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task DeleteAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 设置用户角色
	/// </summary>
	/// <param name="id"></param>
	/// <param name="roles"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task SetRolesAsync(long id, List<string> roles, CancellationToken cancellationToken = default);

	/// <summary>
	/// 修改密码
	/// </summary>
	/// <param name="oldPassword"></param>
	/// <param name="newPassword"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task ChangePasswordAsync(string oldPassword, string newPassword, CancellationToken cancellationToken = default);

	/// <summary>
	/// 重置密码
	/// </summary>
	/// <param name="id"></param>
	/// <param name="password"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task ResetPasswordAsync(long id, string password, CancellationToken cancellationToken = default);
}