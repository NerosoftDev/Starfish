using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IUserInitializeUseCase : IParameterlessUseCase;

internal class UserInitializeUseCase : IUserInitializeUseCase
{
	private readonly IBus _bus;

	public UserInitializeUseCase(IBus bus)
	{
		_bus = bus;
	}

	public async Task ExecuteAsync(CancellationToken cancellationToken = default)
	{
		var userCreateDto = new UserCreateDto
		{
			UserName = "starfish",
			Password = "starfish.888",
			NickName = "admin",
			IsAdmin = true
		};
		var command = TypeAdapter.ProjectedAs<UserCreateCommand>(userCreateDto);
		command.Reserved = true;
		await _bus.SendAsync<UserCreateCommand, string>(command, cancellationToken);
	}
}