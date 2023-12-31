﻿using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

public interface IUserSetRoleUseCase : INonOutputUseCase<UserSetRoleInput>;

public record UserSetRoleInput(int Id, List<string> Roles) : IUseCaseInput;

public class UserSetRoleUseCase : IUserSetRoleUseCase
{
	private readonly IBus _bus;

	public UserSetRoleUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(UserSetRoleInput input, CancellationToken cancellationToken = new CancellationToken())
	{
		var command = new UserSetRoleCommand(input.Id, input.Roles);
		return _bus.SendAsync(command, cancellationToken);
	}
}