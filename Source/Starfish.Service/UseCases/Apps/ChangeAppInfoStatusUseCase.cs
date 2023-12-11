using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

public interface IChangeAppInfoStatusUseCase : IUseCase<ChangeAppInfoStatusInput>;

public record ChangeAppInfoStatusInput(long Id, AppStatus Status) : IUseCaseInput;

public class ChangeAppInfoStatusUseCase : IChangeAppInfoStatusUseCase
{
	/// <summary>
	/// 允许访问的角色
	/// </summary>
	private readonly string[] _roles = { "SA", "RW" };

	private readonly IBus _bus;
	private readonly UserPrincipal _user;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	/// <param name="user"></param>
	public ChangeAppInfoStatusUseCase(IBus bus, UserPrincipal user)
	{
		_bus = bus;
		_user = user;
	}

	/// <inheritdoc />
	public Task ExecuteAsync(ChangeAppInfoStatusInput input, CancellationToken cancellationToken = new CancellationToken())
	{
		if (!_user.IsAuthenticated)
		{
			throw new AuthenticationException();
		}

		if (!_user.IsInRoles(_roles))
		{
			throw new UnauthorizedAccessException();
		}

		var command = new ChangeAppInfoStatusCommand(input.Id, input.Status);
		return _bus.SendAsync(command, cancellationToken);
	}
}