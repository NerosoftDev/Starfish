using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IUserCreateUseCase : IUseCase<UserCreateInput, UserCreateOutput>;

internal record UserCreateOutput(string Result) : IUseCaseOutput;

internal record UserCreateInput(UserCreateDto Data) : IUseCaseInput;

internal class UserCreateUseCase : IUserCreateUseCase
{
	private readonly IBus _bus;

	public UserCreateUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task<UserCreateOutput> ExecuteAsync(UserCreateInput input, CancellationToken cancellationToken = default)
	{
		var command = TypeAdapter.ProjectedAs<UserCreateCommand>(input.Data);
		return _bus.SendAsync<UserCreateCommand, string>(command, cancellationToken)
		           .ContinueWith(task => new UserCreateOutput(task.Result), cancellationToken);
	}
}