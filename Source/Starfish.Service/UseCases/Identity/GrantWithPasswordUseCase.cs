using System.Security.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Common;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 密码登录用例接口
/// </summary>
public interface IGrantWithPasswordUseCase : IUseCase<GrantWithPasswordUseCaseInput, GrantWithPasswordUseCaseOutput>;

/// <summary>
/// 密码登录输入参数
/// </summary>
public class GrantWithPasswordUseCaseInput : IUseCaseInput
{
	/// <summary>
	/// 用户名
	/// </summary>
	public string UserName { get; init; }

	/// <summary>
	/// 密码
	/// </summary>
	public string Password { get; init; }
}

/// <summary>
/// 密码登录输出参数
/// </summary>
public class GrantWithPasswordUseCaseOutput : IdentityUseCaseOutput
{
}

/// <summary>
/// 密码登录用例
/// </summary>
public class GrantWithPasswordUseCase : IGrantWithPasswordUseCase
{
	private readonly IServiceProvider _provider;

	/// <summary>
	/// 初始化<see cref="GrantWithPasswordUseCase"/>.
	/// </summary>
	/// <param name="provider"></param>
	public GrantWithPasswordUseCase(IServiceProvider provider)
	{
		_provider = provider;
	}

	private IUserRepository _userRepository;
	private IUserRepository UserRepository => _userRepository ??= _provider.GetService<IUserRepository>();

	private IdentityCommonComponent _component;
	private IdentityCommonComponent Component => _component ??= _provider.GetService<IdentityCommonComponent>();

	private IBus _bus;
	private IBus Bus => _bus ??= _provider.GetService<IBus>();

	/// <inheritdoc />
	public async Task<GrantWithPasswordUseCaseOutput> ExecuteAsync(GrantWithPasswordUseCaseInput input, CancellationToken cancellationToken = default)
	{
		var events = new List<ApplicationEvent>();
		try
		{
			if (string.IsNullOrWhiteSpace(input.UserName) || string.IsNullOrWhiteSpace(input.Password))
			{
				throw new BadRequestException(Resources.IDS_ERROR_USERNAME_OR_PASSWORD_IS_INVALID);
			}

			var user = await UserRepository.FindByUserNameAsync(input.UserName, false, cancellationToken);
			if (user == null)
			{
				throw new BadRequestException(Resources.IDS_ERROR_USERNAME_OR_PASSWORD_IS_INVALID);
			}

			var passwordHash = Cryptography.DES.Encrypt(input.Password, Encoding.UTF8.GetBytes(user.PasswordSalt));

			if (string.Equals(user.PasswordHash, passwordHash))
			{
				throw new AuthenticationException(Resources.IDS_ERROR_USERNAME_OR_PASSWORD_IS_INVALID);
			}

			if (user.LockoutEnd > DateTime.UtcNow)
			{
				throw new AuthenticationException(Resources.IDS_ERROR_USER_LOCKOUT);
			}

			var roles = user.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
			var (accessToken, refreshToken, issuesAt, expiresAt) = Component.GenerateAccessToken(user.Id, user.UserName, roles);
			@events.Add(new UserAuthSucceedEvent
			{
				AuthType = "password",
				RefreshToken = refreshToken,
				UserId = user.Id,
				UserName = user.UserName,
				TokenIssueTime = issuesAt,
				Data = new Dictionary<string, string>
				{
					{ "username", input.UserName }
				}
			});
			return new GrantWithPasswordUseCaseOutput
			{
				UserId = user.Id,
				AccessToken = accessToken,
				RefreshToken = refreshToken,
				IssuesAt = issuesAt,
				ExpiresAt = expiresAt
			};
		}
		catch (Exception exception)
		{
			@events.Add(new UserAuthFailedEvent
			{
				AuthType = "password",
				Data = new Dictionary<string, string>
				{
					{ "username", input.UserName }
				},
				Error = exception.Message
			});
			throw;
		}
		finally
		{
			if (events.Count > 0)
			{
				await Parallel.ForEachAsync(events, cancellationToken, async (@event, token) => await Bus.PublishAsync(@event, token));
			}
		}
	}
}