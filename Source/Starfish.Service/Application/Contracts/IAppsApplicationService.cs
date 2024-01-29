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
	/// <param name="skip">页码</param>
	/// <param name="count">数量</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<AppInfoItemDto>> QueryAsync(AppInfoCriteria criteria, int skip, int count, CancellationToken cancellationToken = default);

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
	Task<AppInfoDetailDto> GetAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 应用授权
	/// </summary>
	/// <param name="id"></param>
	/// <param name="secret"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> AuthorizeAsync(string id, string secret, CancellationToken cancellationToken = default);

	/// <summary>
	/// 创建应用
	/// </summary>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<string> CreateAsync(AppInfoCreateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 更新应用
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateAsync(string id, AppInfoUpdateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 删除应用
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task DeleteAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 启用应用
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task EnableAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 禁用应用
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task DisableAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 设置应用密钥
	/// </summary>
	/// <param name="id"></param>
	/// <param name="secret"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task SetSecretAsync(string id, string secret, CancellationToken cancellationToken = default);
}