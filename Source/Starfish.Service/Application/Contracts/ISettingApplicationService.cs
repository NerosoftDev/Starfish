using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
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
	Task<List<SettingNodeItemDto>> SearchAsync(SettingNodeCriteria criteria, int page, int size, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取符合条件的配置数量
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<int> CountAsync(SettingNodeCriteria criteria, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取配置详情
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<SettingNodeDetailDto> GetAsync(long id, CancellationToken cancellationToken = default);

	/// <summary>
	/// 新增根节点
	/// </summary>
	/// <param name="appId">应用Id</param>
	/// <param name="environment">环境名称</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<long> CreateRootNodeAsync(long appId, string environment, CancellationToken cancellationToken = default);

	/// <summary>
	/// 新增子节点
	/// </summary>
	/// <param name="parentId"></param>
	/// <param name="type"></param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<long> CreateLeafNodeAsync(long parentId, SettingNodeType type, SettingNodeCreateDto data, CancellationToken cancellationToken = default);

	/// <summary>
	/// 更新配置节点
	/// </summary>
	/// <param name="id"></param>
	/// <param name="value"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateValueAsync(long id, string value, CancellationToken cancellationToken = default);

	/// <summary>
	/// 重命名节点
	/// </summary>
	/// <param name="id"></param>
	/// <param name="name"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateNameAsync(long id, string name, CancellationToken cancellationToken = default);

	/// <summary>
	/// 更新节点描述
	/// </summary>
	/// <param name="id"></param>
	/// <param name="description"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task UpdateDescriptionAsync(long id, string description, CancellationToken cancellationToken = default);

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
	Task PublishAsync(long id, SettingNodePublishDto data, CancellationToken cancellationToken = default);
}