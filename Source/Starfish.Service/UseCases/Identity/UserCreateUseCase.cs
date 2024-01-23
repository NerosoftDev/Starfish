using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface IUserCreateUseCase : IUseCase<UserCreateInput, UserCreateOutput>;

public record UserCreateOutput(long Result) : IUseCaseOutput;

public record UserCreateInput(UserCreateDto Data) : IUseCaseInput;

public class UserCreateUseCase : IUserCreateUseCase
{
	private readonly IBus _bus;

	public UserCreateUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task<UserCreateOutput> ExecuteAsync(UserCreateInput input, CancellationToken cancellationToken = default)
	{
		var command = new UserCreateCommand(input.Data);
		return _bus.SendAsync<UserCreateCommand, long>(command, cancellationToken)
		           .ContinueWith(task => new UserCreateOutput(task.Result), cancellationToken);
	}
}