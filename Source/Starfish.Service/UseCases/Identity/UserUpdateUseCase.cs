using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface IUserUpdateUseCase : IUseCase<UserUpdateInput>;

public record UserUpdateInput(int Id, UserUpdateDto Data) : IUseCaseInput;

public class UserUpdateUseCase : IUserUpdateUseCase
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