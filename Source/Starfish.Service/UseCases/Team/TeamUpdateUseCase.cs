using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface ITeamUpdateUseCase : INonOutputUseCase<TeamUpdateInput>;

internal record TeamUpdateInput(string Id, TeamEditDto Data) : IUseCaseInput;

internal class TeamUpdateUseCase : ITeamUpdateUseCase
{
	private readonly IBus _bus;

	public TeamUpdateUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(TeamUpdateInput input, CancellationToken cancellationToken = default)
	{
		var command = new TeamUpdateCommand(input.Id, input.Data);
		return _bus.SendAsync(command, cancellationToken);
	}
}