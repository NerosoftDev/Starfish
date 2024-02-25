using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

internal interface IAdministratorDeleteUseCase : INonOutputUseCase<AdministratorDeleteInput>;

internal record AdministratorDeleteInput(string UserId) : IUseCaseInput;

internal class AdministratorDeleteUseCase : IAdministratorDeleteUseCase
{
	private readonly IBus _bus;
	private readonly UserPrincipal _user;

	public AdministratorDeleteUseCase(IBus bus, UserPrincipal user)
	{
		_bus = bus;
		_user = user;
	}

	public Task ExecuteAsync(AdministratorDeleteInput input, CancellationToken cancellationToken = default)
	{
		_user.EnsureInRoles(["SA"]);

		if (string.Equals(_user.UserId, input.UserId))
		{
			throw new InvalidOperationException();
		}

		var command = new AdministratorDeleteCommand(input.UserId);

		return _bus.SendAsync(command, cancellationToken);
	}
}