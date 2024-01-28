using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置应用服务
/// </summary>
public interface IConfigurationApplicationService : IApplicationService
{
	/// <summary>
	/// 获取配置项列表
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="skip"></param>
	/// <param name="count"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<ConfigurationItemDto>> GetItemListAsync(long appId, string environment, int skip, int count, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取配置项数量
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<int> GetItemCountAsync(long appId, string environment, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取配置详情
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<ConfigurationDetailDto> GetDetailAsync(long appId, string environment, CancellationToken cancellationToken = default);

	/// <summary>
	/// 新建配置
	/// </summary>
	/// <param name="appId">应用Id</param>
	/// <param name="environment">应用环境</param>
	/// <param name="format"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<long> CreateAsync(long appId, string environment, string format, ConfigurationEditDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 更新配置
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="format"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateAsync(long appId, string environment, string format, ConfigurationEditDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 删除节点
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task DeleteAsync(long appId, string environment, CancellationToken cancellationToken = default);

	/// <summary>
	/// 更新配置项
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateAsync(long appId, string environment, string key, string value, CancellationToken cancellationToken = default);

	/// <summary>
	/// 发布配置
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task PublishAsync(long appId, string environment, ConfigurationPublishDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取已发布的配置
	/// </summary>
	/// <param name="appId">应用Id</param>
	/// <param name="environment">应用环境</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<string> GetArchiveAsync(long appId, string environment, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取指定格式的配置
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="format"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<string> GetItemsInTextAsync(long appId, string environment, string format, CancellationToken cancellationToken = default);

	Task PushRedisAsync(long appId, string environment, PushRedisRequestDto data, CancellationToken cancellationToken = default);
}