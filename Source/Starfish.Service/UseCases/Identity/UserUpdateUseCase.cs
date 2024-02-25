using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IUserUpdateUseCase : INonOutputUseCase<UserUpdateInput>;

internal record UserUpdateInput(string Id, UserUpdateDto Data) : IUseCaseInput;

internal class UserUpdateUseCase : IUserUpdateUseCase
{
	private readonly IBus _bus;

	public UserUpdateUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(UserUpdateInput input, CancellationToken cancellationToken = default)
	{
		var command = new UserUpdateCommand(input.Id, input.Data);
		return _bus.SendAsync(command, cancellationToken);
	}
}