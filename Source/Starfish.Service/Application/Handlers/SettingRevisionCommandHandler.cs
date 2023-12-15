using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;
using Newtonsoft.Json;

namespace Nerosoft.Starfish.Application;

public class SettingRevisionCommandHandler : CommandHandlerBase,
                                             IHandler<SettingRevisionCreateCommand>
{
	public SettingRevisionCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
		: base(unitOfWork, factory)
	{
	}

	public Task HandleAsync(SettingRevisionCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var (appId, appCode, environment, nodes) = await GetNodesAsync(message.SettingId, cancellationToken);

			var entity = new SettingRevision
			{
				AppId = appId,
				AppCode = appCode,
				Environment = environment,
				Version = message.Version,
				Data = JsonConvert.SerializeObject(nodes, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
				Operator = context?.User?.Identity?.Name,
				Comment = message.Comment
			};

			await SaveRevisionAsync(entity, cancellationToken);
		});
	}

	private async Task<Tuple<long, string, string, List<SettingNode>>> GetNodesAsync(long rootId, CancellationToken cancellationToken = default)
	{
		var repository = UnitOfWork.Current.GetService<ISettingNodeRepository>();

		var root = await repository.GetAsync(rootId, false, [], cancellationToken);

		if (root == null)
		{
			throw new SettingNodeNotFoundException(rootId);
		}

		ISpecification<SettingNode>[] specifications =
		{
			SettingNodeSpecification.AppIdEquals(root.AppId),
			SettingNodeSpecification.EnvironmentEquals(root.Environment),
		};

		var predicate = new CompositeSpecification<SettingNode>(PredicateOperator.AndAlso, specifications).Satisfy();

		var leaves = await repository.FindAsync(predicate, false, Array.Empty<string>(), cancellationToken);

		return Tuple.Create(root.AppId, root.AppCode, root.Environment, leaves);
	}

	private Task<SettingRevision> SaveRevisionAsync(SettingRevision entity, CancellationToken cancellationToken = default)
	{
		var repository = UnitOfWork.Current.GetService<ISettingRevisionRepository>();

		return repository.InsertAsync(entity, true, cancellationToken);
	}
}