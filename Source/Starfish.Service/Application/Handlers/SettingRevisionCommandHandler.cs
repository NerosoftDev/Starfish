using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
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
			var nodes = await GetNodesAsync(message.SettingId, cancellationToken);

			var entity = new SettingRevision
			{
				SettingId = message.SettingId,
				Version = message.Version,
				Data = JsonConvert.SerializeObject(nodes, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
				Operator = context?.User?.Identity?.Name,
				Comment = message.Comment
			};

			await SaveRevisionAsync(entity, cancellationToken);
		});
	}

	private async Task<List<SettingNode>> GetNodesAsync(long id, CancellationToken cancellationToken = default)
	{
		var repository = UnitOfWork.Current.GetService<ISettingRepository>();

		var aggregate = await repository.GetAsync(id, false, [nameof(Setting.Nodes)], cancellationToken);

		if (aggregate == null)
		{
			throw new SettingNotFoundException(id);
		}

		return aggregate.Nodes.ToList();
	}

	private Task<SettingRevision> SaveRevisionAsync(SettingRevision entity, CancellationToken cancellationToken = default)
	{
		var repository = UnitOfWork.Current.GetService<ISettingRevisionRepository>();

		return repository.InsertAsync(entity, true, cancellationToken);
	}
}