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
	Task<int> CreateAsync(UserCreateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 编辑用户
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateAsync(int id, UserUpdateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 查询符合条件的用户列表
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<UserItemDto>> SearchAsync(UserCriteria criteria, CancellationToken cancellationToken = default);

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
	Task<UserDetailDto> GetAsync(int id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 删除用户
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}