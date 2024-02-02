using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Application;

public class AdministratorCommandHandler : CommandHandlerBase,
                                           IHandler<AdministratorAssignCommand>,
                                           IHandler<AdministratorDeleteCommand>
{
	public AdministratorCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
		: base(unitOfWork, factory)
	{
	}

	public Task HandleAsync(AdministratorAssignCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<AdministratorGeneralBusiness>(message.UserId, cancellationToken);
			business.UserId = message.UserId;
			business.SetRoles(message.Roles);
			if (business.Aggregate == null)
			{
				business.MarkAsInsert();
			}
			else
			{
				business.MarkAsUpdate();
			}

			_ = await business.SaveAsync(false, cancellationToken);
		});
	}

	public Task HandleAsync(AdministratorDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<AdministratorGeneralBusiness>(message.UserId, cancellationToken);

			business.MarkAsDelete();

			_ = await business.SaveAsync(false, cancellationToken);
		});
	}
}