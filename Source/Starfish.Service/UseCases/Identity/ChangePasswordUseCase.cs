using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

public interface IChangePasswordUseCase : INonOutputUseCase<ChangePasswordInput>;

public record ChangePasswordInput(string OldPassword, string NewPassword) : IUseCaseInput;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
	private readonly IUserRepository _repository;
	private readonly IBus _bus;
	private readonly UserPrincipal _user;

	public ChangePasswordUseCase(IUserRepository repository, IBus bus, UserPrincipal user)
	{
		_repository = repository;
		_bus = bus;
		_user = user;
	}

	public async Task ExecuteAsync(ChangePasswordInput input, CancellationToken cancellationToken = default)
	{
		if (!_user.IsAuthenticated)
		{
			throw new AuthenticationException();
		}

		var user = await _repository.GetAsync(_user.UserId, null, cancellationToken);

		if (user == null)
		{
			throw new BadRequestException(Resources.IDS_ERROR_USER_NOT_EXISTS);
		}

		var passwordHash = Cryptography.DES.Encrypt(input.OldPassword, Encoding.UTF8.GetBytes(user.PasswordSalt));

		if (!string.Equals(user.PasswordHash, passwordHash))
		{
			throw new BadRequestException(Resources.IDS_ERROR_PASSWORD_INCORRECT);
		}

		var command = new ChangePasswordCommand(user.Id, input.NewPassword);
		await _bus.SendAsync(command, cancellationToken);
	}
}