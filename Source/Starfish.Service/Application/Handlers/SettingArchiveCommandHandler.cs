using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
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
			var (appId, appCode, environment, nodes) = await GetNodesAsync(message.SettingId, cancellationToken);
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

		var validTypes = new[] { SettingNodeType.String, SettingNodeType.Number, SettingNodeType.Boolean, SettingNodeType.Referer };
		var leaves = await repository.GetNodesAsync(root.AppId, root.Environment, validTypes, cancellationToken);

		return Tuple.Create(root.AppId, root.AppCode, root.Environment, leaves);
	}

	private async Task SaveArchiveAsync(long appId, string appCode, string environment, string data, string userName, CancellationToken cancellationToken = default)
	{
		var repository = UnitOfWork.Current.GetService<ISettingArchiveRepository>();

		var archive = await repository.GetAsync(appCode, environment, cancellationToken);

		archive ??= new SettingArchive
		{
			AppId = appId,
			AppCode = appCode,
			Environment = environment,
		};

		archive.Data = data;
		archive.Operator = userName;
		archive.ArchiveTime = DateTime.Now;

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