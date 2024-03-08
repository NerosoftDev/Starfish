using Microsoft.Extensions.Configuration;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

internal interface IUserInitializeUseCase : IParameterlessUseCase;

internal class UserInitializeUseCase : IUserInitializeUseCase
{
	private readonly IBus _bus;
	private readonly IUserRepository _repository;
	private readonly IConfiguration _configuration;

	public UserInitializeUseCase(IBus bus, IUserRepository repository, IConfiguration configuration)
	{
		_bus = bus;
		_repository = repository;
		_configuration = configuration;
	}

	public async Task ExecuteAsync(CancellationToken cancellationToken = default)
	{
		var username = _configuration["InitializeUser:UserName"];

		if (string.IsNullOrWhiteSpace(username))
		{
			return;
		}

		var exists = await _repository.CheckUserNameExistsAsync(username, cancellationToken);
		if (exists)
		{
			return;
		}

		var command = new UserCreateCommand
		{
			UserName = _configuration["InitializeUser:UserName"],
			Password = _configuration["InitializeUser:Password"],
			IsAdmin = true,
			Reserved = true
		};
		await _bus.SendAsync<UserCreateCommand, string>(command, cancellationToken)
		          .ContinueWith(task => task.WaitAndUnwrapException(), cancellationToken);
	}
}