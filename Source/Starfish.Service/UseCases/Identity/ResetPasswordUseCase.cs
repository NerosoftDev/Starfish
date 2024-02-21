using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

internal interface IResetPasswordUseCase : INonOutputUseCase<ResetPasswordInput>;

internal record ResetPasswordInput(string Id, string Password) : IUseCaseInput;

internal class ResetPasswordUseCase : IResetPasswordUseCase
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