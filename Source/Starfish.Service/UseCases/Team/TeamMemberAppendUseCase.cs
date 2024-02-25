using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

internal interface ITeamMemberAppendUseCase : INonOutputUseCase<TeamMemberAppendInput>;

internal record TeamMemberAppendInput(string Id, List<string> UserIds) : IUseCaseInput;

internal class TeamMemberAppendUseCase : ITeamMemberAppendUseCase
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