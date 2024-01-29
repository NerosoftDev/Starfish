using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

public interface ITeamMemberAppendUseCase : INonOutputUseCase<TeamMemberAppendInput>;

public record TeamMemberAppendInput(string Id, List<string> UserIds) : IUseCaseInput;

public class TeamMemberAppendUseCase : ITeamMemberAppendUseCase
{
	private readonly IBus _bus;

	public TeamMemberAppendUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(TeamMemberAppendInput input, CancellationToken cancellationToken = default)
	{
		var command = new TeamMemberEditCommand(input.Id, input.UserIds, "+");
		return _bus.SendAsync(command, cancellationToken);
	}
}