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
	public Task<bool> CheckChildNodeNameAsync(long id, long parentId, string name, CancellationToken cancellationToken = default)
	{
		var specification = SettingNodeSpecification.ParentIdEquals(parentId)
		                    & SettingNodeSpecification.NameEquals(name)
		                    & SettingNodeSpecification.IdNotEquals(id);
		var predicate = specification.Satisfy();
		var set = Context.Set<SettingNode>()
		                 .AsNoTracking();
		return set.AnyAsync(predicate, cancellationToken);
	}

	/// <inheritdoc />
	public override Task<SettingNode> GetAsync(long id, CancellationToken cancellationToken = default)
	{
		var specification = SettingNodeSpecification.IdEquals(id);
		var predicate = specification.Satisfy();
		var set = Context.Set<SettingNode>()
		                 .Include(t => t.Children);
		return set.FirstOrDefaultAsync(predicate, cancellationToken);
	}
}