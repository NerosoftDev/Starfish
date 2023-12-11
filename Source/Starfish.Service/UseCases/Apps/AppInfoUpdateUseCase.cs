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
public interface IAppInfoUpdateUseCase : IUseCase<AppInfoUpdateInput>;

/// <summary>
/// 应用信息更新输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Model"></param>
public record AppInfoUpdateInput(long Id, AppInfoUpdateDto Model) : IUseCaseInput;

/// <summary>
/// 应用信息更新用例
/// </summary>
public class AppInfoUpdateUseCase : IAppInfoUpdateUseCase
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
	public AppInfoUpdateUseCase(IBus bus, UserPrincipal user)
	{
		_bus = bus;
		_user = user;
	}

	/// <inheritdoc />
	public Task ExecuteAsync(AppInfoUpdateInput input, CancellationToken cancellationToken = default)
	{
		if (!_user.IsAuthenticated)
		{
			throw new AuthenticationException();
		}

		if (!_user.IsInRoles(_roles))
		{
			throw new UnauthorizedAccessException();
		}

		var command = new AppInfoUpdateCommand(input.Id, input.Model);
		return _bus.SendAsync(command, cancellationToken);
	}
}