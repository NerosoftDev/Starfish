using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface ITeamCreateUseCase : IUseCase<TeamCreateInput, TeamCreateOutput>;

public record TeamCreateInput(TeamEditDto Data) : IUseCaseInput;

public record TeamCreateOutput(string Result) : IUseCaseOutput;

public class TeamCreateUseCase : ITeamCreateUseCase
{
	private readonly IBus _bus;

	public TeamCreateUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task<TeamCreateOutput> ExecuteAsync(TeamCreateInput input, CancellationToken cancellationToken = default)
	{
		var command = new TeamCreateCommand(input.Data);
		return _bus.SendAsync<TeamCreateCommand, string>(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
			           return new TeamCreateOutput(task.Result);
		           }, cancellationToken);
	}
}