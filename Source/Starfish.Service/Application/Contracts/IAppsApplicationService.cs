using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用信息服务接口
/// </summary>
public interface IAppsApplicationService : IApplicationService
{
	/// <summary>
	/// 获取符合条件的应用列表
	/// </summary>
	/// <param name="criteria">查询条件</param>
	/// <param name="page">页码</param>
	/// <param name="size">数量</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<AppInfoItemDto>> SearchAsync(AppInfoCriteria criteria, int page, int size, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取符合条件的应用数量
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<int> CountAsync(AppInfoCriteria criteria, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取应用详情
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<AppInfoDetailDto> GetAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 应用授权
	/// </summary>
	/// <param name="code"></param>
	/// <param name="secret"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> AuthorizeAsync(string code, string secret, CancellationToken cancellationToken = default);

	/// <summary>
	/// 创建应用
	/// </summary>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<long> CreateAsync(AppInfoCreateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 更新应用
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateAsync(long id, AppInfoUpdateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 删除应用
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task DeleteAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 启用应用
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task EnableAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 禁用应用
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task DisableAsync(long id, CancellationToken cancellationToken = default);
}