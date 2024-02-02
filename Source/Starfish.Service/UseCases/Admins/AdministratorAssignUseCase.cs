using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Claims;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IAdministratorAssignUseCase : INonOutputUseCase<AdministratorAssignInput>;

internal record AdministratorAssignInput(AdministratorAssignDto Data);

internal class AdministratorAssignUseCase : IAdministratorAssignUseCase
{
	private readonly IBus _bus;
	private readonly UserPrincipal _user;

	public AdministratorAssignUseCase(IBus bus, UserPrincipal user)
	{
		_bus = bus;
		_user = user;
	}

	public Task ExecuteAsync(AdministratorAssignInput input, CancellationToken cancellationToken = default)
	{
		_user.EnsureInRoles(["SA"]);

		if (string.Equals(_user.UserId, input.Data.UserId))
		{
			throw new InvalidOperationException();
		}

		var command = TypeAdapter.ProjectedAs<AdministratorAssignCommand>(input.Data);
		return _bus.SendAsync(command, cancellationToken);
	}
}