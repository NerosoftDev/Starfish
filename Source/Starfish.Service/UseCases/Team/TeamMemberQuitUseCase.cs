using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

public interface ITeamMemberQuitUseCase : INonOutputUseCase<TeamMemberQuitInput>;

public record TeamMemberQuitInput(long TeamId, long UserId) : IUseCaseInput;

public class TeamMemberQuitUseCase : ITeamMemberQuitUseCase
{
	private readonly IBus _bus;

	public TeamMemberQuitUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(TeamMemberQuitInput input, CancellationToken cancellationToken = default)
	{
		var command = new TeamMemberEditCommand(input.TeamId, [input.UserId], "-");
		return _bus.SendAsync(command, cancellationToken);
	}
}