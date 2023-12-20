using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

public interface IAppInfoSetSecretUseCase : IUseCase<AppInfoSetSecretInput>;

public record AppInfoSetSecretInput(long Id, string Secret) : IUseCaseInput;

public class AppInfoSetSecretUseCase : IAppInfoSetSecretUseCase
{
	private readonly IBus _bus;

	public AppInfoSetSecretUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(AppInfoSetSecretInput input, CancellationToken cancellationToken = new CancellationToken())
	{
		var command = new AppInfoSetSecretCommand(input.Id, input.Secret);
		return _bus.SendAsync(command, cancellationToken);
	}
}