using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置应用服务
/// </summary>
public interface ISettingApplicationService : IApplicationService
{
	/// <summary>
	/// 获取符合条件的配置列表
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="page"></param>
	/// <param name="size"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<SettingItemDto>> SearchAsync(SettingCriteria criteria, int page, int size, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取符合条件的配置数量
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<int> CountAsync(SettingCriteria criteria, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取配置详情
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<SettingDetailDto> GetAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 新建配置
	/// </summary>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<long> CreateAsync(SettingCreateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 删除节点
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task DeleteAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 发布配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task PublishAsync(long id, SettingPublishDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取已发布的配置
	/// </summary>
	/// <param name="appId">应用Id</param>
	/// <param name="environment">环境名称</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<string> GetSettingRawAsync(long appId, string environment, CancellationToken cancellationToken = default);
}