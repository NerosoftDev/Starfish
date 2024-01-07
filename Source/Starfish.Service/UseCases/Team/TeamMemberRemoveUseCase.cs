using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

public interface ITeamMemberRemoveUseCase : INonOutputUseCase<TeamMemberRemoveInput>;

public record TeamMemberRemoveInput(int Id, List<int> UserIds) : IUseCaseInput;

public class TeamMemberRemoveUseCase : ITeamMemberRemoveUseCase
{
	private readonly IBus _bus;

	public TeamMemberRemoveUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(TeamMemberRemoveInput input, CancellationToken cancellationToken = new CancellationToken())
	{
		var command = new TeamMemberEditCommand(input.Id, input.UserIds, "-");
		return _bus.SendAsync(command, cancellationToken);
	}
}