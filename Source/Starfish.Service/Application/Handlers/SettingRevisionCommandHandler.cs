using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

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
			var repository = UnitOfWork.Current.GetService<ISettingRepository>();

			var aggregate = await repository.GetAsync(message.AppId, message.Environment, false, [nameof(Setting.Items), nameof(Setting.Revisions)], cancellationToken);

			if (aggregate == null)
			{
				throw new SettingNotFoundException(message.AppId, message.Environment);
			}

			aggregate.CreateRevision(message.Version, message.Comment, context.User?.Identity?.Name);
		});
	}
}