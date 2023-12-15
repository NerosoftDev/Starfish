using System.Linq.Expressions;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 设置节点仓储
/// </summary>
public interface ISettingNodeRepository : IRepository<SettingNode, long>
{
	/// <summary>
	/// 检查根节点配置是否存在
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> ExistsAsync(long appId, string environment, CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取指定节点
	/// </summary>
	/// <param name="id"></param>
	/// <param name="tracking"></param>
	/// <param name="properties"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<SettingNode> GetAsync(long id, bool tracking, string[] properties, CancellationToken cancellationToken = default);

	/// <summary>
	/// 根据Key查询所有叶子节点
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="key"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<SettingNode>> GetLeavesAsync(long appId, string environment, string key, CancellationToken cancellationToken = default);

	/// <summary>
	/// 根据应用Id和环境查询所有节点
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<SettingNode>> GetNodesAsync(long appId, string environment, CancellationToken cancellationToken = default);

	/// <summary>
	/// 根据应用Id和环境查询所有节点
	/// </summary>
	/// <param name="appId"></param>
	/// <param name="environment"></param>
	/// <param name="types"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<SettingNode>> GetNodesAsync(long appId, string environment, IEnumerable<SettingNodeType> types, CancellationToken cancellationToken = default);

	/// <summary>
	/// 查询同级节点名称是否存在
	/// </summary>
	/// <param name="id"></param>
	/// <param name="parentId"></param>
	/// <param name="name"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> CheckChildNodeNameAsync(long id, long parentId, string name, CancellationToken cancellationToken = default);

	/// <summary>
	/// 查询符合条件的节点列表
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="tracking"></param>
	/// <param name="properties"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<SettingNode>> FindAsync(Expression<Func<SettingNode, bool>> predicate, bool tracking, string[] properties, CancellationToken cancellationToken = default);
}