using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

public interface IResetPasswordUseCase : INonOutputUseCase<ResetPasswordInput>;

public record ResetPasswordInput(int Id, string Password) : IUseCaseInput;

public class ResetPasswordUseCase : IResetPasswordUseCase
{
	private readonly IBus _bus;

	public ResetPasswordUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(ResetPasswordInput input, CancellationToken cancellationToken = default)
	{
		var command = new ChangePasswordCommand(input.Id, input.Password);
		return _bus.SendAsync(command, cancellationToken);
	}
}