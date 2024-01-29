using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 应用信息删除用例接口
/// </summary>
public interface IAppInfoDeleteUseCase : INonOutputUseCase<AppInfoDeleteInput>;

/// <summary>
/// 应用信息删除输入
/// </summary>
/// <param name="Id"></param>
public record AppInfoDeleteInput(string Id) : IUseCaseInput;

/// <summary>
/// 应用信息删除用例
/// </summary>
public class AppInfoDeleteUseCase : IAppInfoDeleteUseCase
{
	/// <summary>
	/// 允许访问的角色
	/// </summary>
	private readonly string[] _roles = ["SA", "RW"];

	private readonly IBus _bus;
	private readonly UserPrincipal _user;

	public AppInfoDeleteUseCase(IBus bus, UserPrincipal user)
	{
		_bus = bus;
		_user = user;
	}

	public Task ExecuteAsync(AppInfoDeleteInput input, CancellationToken cancellationToken = default)
	{
		if (!_user.IsAuthenticated)
		{
			throw new AuthenticationException();
		}

		if (!_user.IsInRoles(_roles))
		{
			throw new UnauthorizedAccessException();
		}

		var command = new AppInfoDeleteCommand(input.Id);
		return _bus.SendAsync(command, cancellationToken);
	}
}