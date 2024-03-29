﻿using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

internal interface ITeamMemberQuitUseCase : INonOutputUseCase<TeamMemberQuitInput>;

internal record TeamMemberQuitInput(string TeamId, string UserId) : IUseCaseInput;

internal class TeamMemberQuitUseCase : ITeamMemberQuitUseCase
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