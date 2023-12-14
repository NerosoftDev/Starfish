using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 设置节点仓储
/// </summary>
public class SettingNodeRepository : BaseRepository<DataContext, SettingNode, long>, ISettingNodeRepository
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="provider"></param>
	public SettingNodeRepository(IContextProvider provider)
		: base(provider)
	{
	}

	/// <inheritdoc />
	public Task<bool> ExistsAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var specification = SettingNodeSpecification.AppIdEquals(appId) & SettingNodeSpecification.EnvironmentEquals(environment);
		var predicate = specification.Satisfy();
		return base.ExistsAsync(predicate, cancellationToken);
	}

	/// <inheritdoc />
	public Task<List<SettingNode>> GetLeavesAsync(long appId, string environment, string key, CancellationToken cancellationToken = default)
	{
		var specification = SettingNodeSpecification.KeyStartsWith($"{key}:");
		specification &= SettingNodeSpecification.AppIdEquals(appId);
		specification &= SettingNodeSpecification.EnvironmentEquals(environment);
		var predicate = specification.Satisfy();
		return base.FindAsync(predicate, false, Array.Empty<string>(), cancellationToken);
	}

	/// <inheritdoc />
	public async Task<List<SettingNode>> GetNodesAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var specification = SettingNodeSpecification.AppIdEquals(appId) & SettingNodeSpecification.EnvironmentEquals(environment);
		var predicate = specification.Satisfy();
		var set = Context.Set<SettingNode>()
		                 .Include(t => t.Children);
		return await set.Where(predicate)
		                .ToListAsync(cancellationToken);
	}

	/// <inheritdoc />
	public Task<List<SettingNode>> GetNodesAsync(long appId, string environment, IEnumerable<SettingNodeType> types, CancellationToken cancellationToken = default)
	{
		var specification = SettingNodeSpecification.AppIdEquals(appId) & SettingNodeSpecification.EnvironmentEquals(environment) & SettingNodeSpecification.TypeIn(types);
		var predicate = specification.Satisfy();
		return base.FindAsync(predicate, false, Array.Empty<string>(), cancellationToken);
	}

	/// <inheritdoc />
	public Task<List<SettingNode>> GetNodesAsync(string appCode, string environment, IEnumerable<SettingNodeType> types, CancellationToken cancellationToken = default)
	{
		var specification = SettingNodeSpecification.AppCodeEquals(appCode) & SettingNodeSpecification.EnvironmentEquals(environment) & SettingNodeSpecification.TypeIn(types);
		var predicate = specification.Satisfy();
		return base.FindAsync(predicate, false, Array.Empty<string>(), cancellationToken);
	}

	/// <inheritdoc />
	public override Task<SettingNode> GetAsync(long key, CancellationToken cancellationToken = default)
	{
		var specification = SettingNodeSpecification.IdEquals(key);
		var predicate = specification.Satisfy();
		var set = Context.Set<SettingNode>()
		                 .Include(t => t.Children);
		return set.FirstOrDefaultAsync(predicate, cancellationToken);
	}
}