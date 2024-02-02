using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 应用信息更新用例接口
/// </summary>
public interface IAppInfoUpdateUseCase : INonOutputUseCase<AppInfoUpdateInput>;

/// <summary>
/// 应用信息更新输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Model"></param>
public record AppInfoUpdateInput(string Id, AppInfoUpdateDto Model) : IUseCaseInput;

/// <summary>
/// 应用信息更新用例
/// </summary>
public class AppInfoUpdateUseCase : IAppInfoUpdateUseCase
{
	private readonly IBus _bus;
	private readonly UserPrincipal _user;

	public AppInfoUpdateUseCase(IBus bus, UserPrincipal user)
	{
		_bus = bus;
		_user = user;
	}

	public Task ExecuteAsync(AppInfoUpdateInput input, CancellationToken cancellationToken = default)
	{
		if (!_user.IsAuthenticated)
		{
			throw new AuthenticationException();
		}

		var command = new AppInfoUpdateCommand(input.Id, input.Model);
		return _bus.SendAsync(command, cancellationToken);
	}
}