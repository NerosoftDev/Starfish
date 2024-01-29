using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface ITeamUpdateUseCase : INonOutputUseCase<TeamUpdateInput>;

public record TeamUpdateInput(string Id, TeamEditDto Data) : IUseCaseInput;

public class TeamUpdateUseCase : ITeamUpdateUseCase
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