using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置应用服务
/// </summary>
public interface IConfigurationApplicationService : IApplicationService
{
	Task<List<ConfigurationDto>> QueryAsync(ConfigurationCriteria criteria, int skip, int count, CancellationToken cancellationToken = default);

	Task<int> CountAsync(ConfigurationCriteria criteria, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取配置项列表
	/// </summary>
	/// <param name="id"></param>
	/// <param name="key"></param>
	/// <param name="skip"></param>
	/// <param name="count"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<ConfigurationItemDto>> GetItemListAsync(long id, string key, int skip, int count, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取配置项数量
	/// </summary>
	/// <param name="id"></param>
	/// <param name="key"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<int> GetItemCountAsync(long id, string key, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取配置详情
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<ConfigurationDto> GetDetailAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 新建配置
	/// </summary>
	/// <param name="teamId">团队Id</param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<long> CreateAsync(long teamId, ConfigurationEditDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 更新配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateAsync(long id, ConfigurationEditDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 删除节点
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task DeleteAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 设置访问密钥
	/// </summary>
	/// <param name="id"></param>
	/// <param name="secret"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task SetSecretAsync(long id, string secret, CancellationToken cancellationToken = default);

	/// <summary>
	/// 禁用配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task DisableAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 启用配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task EnableAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 应用认证
	/// </summary>
	/// <param name="id"></param>
	/// <param name="name"></param>
	/// <param name="secret"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<long> AuthorizeAsync(long id, string name, string secret, CancellationToken cancellationToken = default);

	/// <summary>
	/// 更新配置项
	/// </summary>
	/// <param name="id"></param>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateValueAsync(long id, string key, string value, CancellationToken cancellationToken = default);

	/// <summary>
	/// 批量更新配置项
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateItemsAsync(long id, ConfigurationItemsUpdateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 发布配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task PublishAsync(long id, ConfigurationPublishRequestDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取已发布的配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<string> GetArchiveAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取指定格式的配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="format"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<string> GetItemsInTextAsync(long id, string format, CancellationToken cancellationToken = default);

	Task PushRedisAsync(long id, ConfigurationPushRedisRequestDto data, CancellationToken cancellationToken = default);
}