using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

public interface IUserDeleteUseCase : INonOutputUseCase<UserDeleteInput>;

public record UserDeleteInput(long Id) : IUseCaseInput;

public class UserDeleteUseCase : IUserDeleteUseCase
{
	private readonly IBus _bus;

	public UserDeleteUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(UserDeleteInput input, CancellationToken cancellationToken = default)
	{
		var command = new UserDeleteCommand(input.Id);
		return _bus.SendAsync(command, cancellationToken);
	}
}