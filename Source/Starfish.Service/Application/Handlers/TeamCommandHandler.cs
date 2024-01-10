using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Application;

public class TeamCommandHandler : CommandHandlerBase,
                                  IHandler<TeamCreateCommand>,
                                  IHandler<TeamUpdateCommand>,
                                  IHandler<TeamMemberEditCommand>
{
	public TeamCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
		: base(unitOfWork, factory)
	{
	}

	public Task HandleAsync(TeamCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.CreateAsync<TeamGeneralBusiness>(cancellationToken);
			business.Alias = message.Data.Alias;
			business.Name = message.Data.Name;
			business.Description = message.Data.Description;
			business.MarkAsInsert();
			_ = await business.SaveAsync(false, cancellationToken);
			return business.Id;
		}, context.Response);
	}

	public Task HandleAsync(TeamUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.FetchAsync<TeamGeneralBusiness>(message.Id, cancellationToken);
			business.Alias = message.Data.Alias;
			business.Name = message.Data.Name;
			business.Description = message.Data.Description;
			business.MarkAsUpdate();
			_ = await business.SaveAsync(true, cancellationToken);
		});
	}

	public Task HandleAsync(TeamMemberEditCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			if (message.UserIds == null || message.UserIds.Count == 0)
			{
				throw new ArgumentException("UserIds is empty");
			}

			var business = await Factory.FetchAsync<TeamMemberBusiness>(message.TeamId, cancellationToken);
			business.UserIds = message.UserIds.ToArray();
			switch (message.Type)
			{
				case "+":
					business.MarkAsInsert();
					break;
				case "-":
					business.MarkAsDelete();
					break;
				default:
					throw new InvalidOperationException();
			}

			_ = await business.SaveAsync(false, cancellationToken);
		});
	}
}