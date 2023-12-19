using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Repository;

// ReSharper disable UnusedMember.Global

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用配置发布领域服务
/// </summary>
public class SettingPublishBusiness : CommandObject<SettingPublishBusiness>, IDomainService
{
	private readonly ISettingNodeRepository _repository;

	public SettingPublishBusiness(ISettingNodeRepository repository)
	{
		_repository = repository;
	}

	[FactoryExecute]
	protected async Task ExecuteAsync(long id, CancellationToken cancellationToken = default)
	{
		var aggregate = await _repository.GetAsync(id, true, Array.Empty<string>(), cancellationToken);

		if (aggregate == null)
		{
			throw new SettingNodeNotFoundException(id);
		}

		if (aggregate == null)
		{
			throw new SettingNodeNotFoundException(id);
		}

		if (aggregate.Type != SettingNodeType.Root)
		{
			throw new InvalidOperationException(Resources.IDS_ERROR_ONLY_ALLOW_PUBLISH_ROOT_NODE);
		}

		List<SettingNode> nodes = [aggregate];

		var types = new[]
		{
			SettingNodeType.String,
			SettingNodeType.Boolean,
			SettingNodeType.Number,
			SettingNodeType.Referer
		};

		ISpecification<SettingNode>[] specifications =
		{
			SettingNodeSpecification.AppIdEquals(aggregate.AppId),
			SettingNodeSpecification.EnvironmentEquals(aggregate.Environment),
			SettingNodeSpecification.TypeIn(types)
		};

		var predicate = new CompositeSpecification<SettingNode>(PredicateOperator.AndAlso, specifications).Satisfy();

		var leaves = await _repository.FindAsync(predicate, false, Array.Empty<string>(), cancellationToken);
		nodes.AddRange(leaves);

		foreach (var node in nodes)
		{
			node.ClearEvents();
		}

		await _repository.UpdateAsync(nodes, true, cancellationToken);
	}
}