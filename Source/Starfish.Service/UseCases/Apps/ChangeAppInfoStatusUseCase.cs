using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 变更应用信息状态用例接口
/// </summary>
public interface IChangeAppInfoStatusUseCase : INonOutputUseCase<ChangeAppInfoStatusInput>;

/// <summary>
/// 变更应用信息状态输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Status"></param>
public record ChangeAppInfoStatusInput(long Id, AppStatus Status) : IUseCaseInput;

/// <summary>
/// 变更应用信息状态用例
/// </summary>
public class ChangeAppInfoStatusUseCase : IChangeAppInfoStatusUseCase
{
	private readonly IBus _bus;
	private readonly UserPrincipal _user;

	public ChangeAppInfoStatusUseCase(IBus bus, UserPrincipal user)
	{
		_bus = bus;
		_user = user;
	}

	public Task ExecuteAsync(ChangeAppInfoStatusInput input, CancellationToken cancellationToken = default)
	{
		if (!_user.IsAuthenticated)
		{
			throw new AuthenticationException();
		}

		var command = new ChangeAppStatusCommand(input.Id, input.Status);
		return _bus.SendAsync(command, cancellationToken);
	}
}