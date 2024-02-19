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
	/// <param name="skip"></param>
	/// <param name="count"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<ConfigurationItemDto>> GetItemListAsync(string id, int skip, int count, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取配置项数量
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<int> GetItemCountAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取配置详情
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<ConfigurationDto> GetDetailAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 新建配置
	/// </summary>
	/// <param name="teamId">团队Id</param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<string> CreateAsync(string teamId, ConfigurationEditDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 更新配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateAsync(string id, ConfigurationEditDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 删除节点
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task DeleteAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 设置访问密钥
	/// </summary>
	/// <param name="id"></param>
	/// <param name="secret"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task SetSecretAsync(string id, string secret, CancellationToken cancellationToken = default);

	/// <summary>
	/// 禁用配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task DisableAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 启用配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task EnableAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 应用认证
	/// </summary>
	/// <param name="id"></param>
	/// <param name="teamId"></param>
	/// <param name="name"></param>
	/// <param name="secret"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<string> AuthorizeAsync(string id, string teamId, string name, string secret, CancellationToken cancellationToken = default);

	/// <summary>
	/// 更新配置项
	/// </summary>
	/// <param name="id"></param>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateValueAsync(string id, string key, string value, CancellationToken cancellationToken = default);

	/// <summary>
	/// 批量更新配置项
	/// </summary>
	/// <param name="id"></param>
	/// <param name="format"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateItemsAsync(string id, ConfigurationItemsUpdateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 发布配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task PublishAsync(string id, ConfigurationPublishRequestDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取已发布的配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<string> GetArchiveAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取指定格式的配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="format"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<string> GetItemsInTextAsync(string id, string format, CancellationToken cancellationToken = default);

	Task PushRedisAsync(string id, ConfigurationPushRedisRequestDto data, CancellationToken cancellationToken = default);
}