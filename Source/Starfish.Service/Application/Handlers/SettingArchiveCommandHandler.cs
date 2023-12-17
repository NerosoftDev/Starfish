using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;
using Newtonsoft.Json;

namespace Nerosoft.Starfish.Application;

public class SettingArchiveCommandHandler : CommandHandlerBase,
											IHandler<SettingArchiveCreateCommand>
{
	public SettingArchiveCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
		: base(unitOfWork, factory)
	{
	}

	public Task HandleAsync(SettingArchiveCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var (appId, appCode, environment, nodes) = await GetNodesAsync(message.RootId, cancellationToken);
			var data = nodes.ToDictionary(t => t.Key, t => t.Value);
			var json = JsonConvert.SerializeObject(data);

			var userName = context?.User?.Identity?.Name;

			await SaveArchiveAsync(appId, appCode, environment, json, userName, cancellationToken);
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

		var types = new[]
		{
			SettingNodeType.String,
			SettingNodeType.Boolean,
			SettingNodeType.Number,
			SettingNodeType.Referer
		};

		ISpecification<SettingNode>[] specifications =
		{
			SettingNodeSpecification.AppIdEquals(root.AppId),
			SettingNodeSpecification.EnvironmentEquals(root.Environment),
			SettingNodeSpecification.TypeIn(types),
			SettingNodeSpecification.StatusEquals(SettingNodeStatus.Pending)
		};

		var predicate = new CompositeSpecification<SettingNode>(PredicateOperator.AndAlso, specifications).Satisfy();

		var leaves = await repository.FindAsync(predicate, false, [], cancellationToken);

		return Tuple.Create(root.AppId, root.AppCode, root.Environment, leaves);
	}

	private async Task SaveArchiveAsync(long appId, string appCode, string environment, string data, string userName, CancellationToken cancellationToken = default)
	{
		var repository = UnitOfWork.Current.GetService<ISettingArchiveRepository>();

		var archive = await repository.GetAsync(appCode, environment, cancellationToken);

		archive ??= SettingArchive.Create(appId, appCode, environment);

		archive.Update(data, userName);

		if (archive.Id > 0)
		{
			await repository.UpdateAsync(archive, true, cancellationToken);
		}
		else
		{
			await repository.InsertAsync(archive, true, cancellationToken);
		}
	}
}