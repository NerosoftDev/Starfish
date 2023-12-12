using System.Security.Authentication;
using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 刷新令牌用例接口
/// </summary>
public interface IGrantWithRefreshTokenUseCase : IUseCase<GrantWithRefreshTokenUseCaseInput, GrantWithRefreshTokenUseCaseOutput>;

/// <summary>
/// 刷新令牌输入参数
/// </summary>
public class GrantWithRefreshTokenUseCaseInput : IUseCaseInput
{
	/// <summary>
	/// 刷新令牌
	/// </summary>
	public string Token { get; set; }
}

/// <summary>
/// 刷新令牌输出
/// </summary>
public class GrantWithRefreshTokenUseCaseOutput : IdentityUseCaseOutput
{
}

/// <summary>
/// 刷新令牌用例
/// </summary>
public class GrantWithRefreshTokenUseCase : IGrantWithRefreshTokenUseCase
{
	private readonly IServiceProvider _provider;

	/// <summary>
	/// 初始化<see cref="GrantWithRefreshTokenUseCase"/>.
	/// </summary>
	/// <param name="provider"></param>
	public GrantWithRefreshTokenUseCase(IServiceProvider provider)
	{
		_provider = provider;
	}

	private IUserRepository UserRepository => _provider.GetService<IUserRepository>();
	private ITokenRepository TokenRepository => _provider.GetService<ITokenRepository>();

	private IdentityCommonComponent _component;
	private IdentityCommonComponent Component => _component ??= _provider.GetService<IdentityCommonComponent>();

	private IBus _bus;
	private IBus Bus => _bus ??= _provider.GetService<IBus>();

	/// <inheritdoc />
	public async Task<GrantWithRefreshTokenUseCaseOutput> ExecuteAsync(GrantWithRefreshTokenUseCaseInput input, CancellationToken cancellationToken = default)
	{
		var events = new List<ApplicationEvent>();
		try
		{
			if (string.IsNullOrWhiteSpace(input.Token))
			{
				throw new BadRequestException(Resources.IDS_ERROR_REFRESH_TOKEN_REQUIRED);
			}

			var key = input.Token.ToSha256();

			var token = await TokenRepository.GetAsync(t => t.Key == key, false, cancellationToken);

			if (token == null)
			{
				throw new BadRequestException(Resources.IDS_ERROR_REFRESH_TOKEN_IS_INVALID);
			}

			if (token.Expires < DateTime.UtcNow)
			{
				throw new BadRequestException(Resources.IDS_ERROR_REFRESH_TOKEN_EXPIRED);
			}

			var user = await UserRepository.GetAsync(int.Parse(token.Subject), false, cancellationToken);

			if (user == null)
			{
				throw new BadRequestException(Resources.IDS_ERROR_REFRESH_TOKEN_IS_INVALID);
			}

			if (user.LockoutEnd > DateTime.UtcNow)
			{
				throw new AuthenticationException(Resources.IDS_ERROR_USER_LOCKOUT);
			}

			var roles = user.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
			var (accessToken, refreshToken, issuesAt, expiresAt) = Component.GenerateAccessToken(user.Id, user.UserName, roles);
			@events.Add(new UserAuthSucceedEvent
			{
				AuthType = "refresh_token",
				RefreshToken = refreshToken,
				UserId = user.Id,
				UserName = user.UserName,
				TokenIssueTime = issuesAt,
				Data = new Dictionary<string, string>
				{
					{ "refresh_token", input.Token }
				}
			});
			@events.Add(new TokenRefreshedEvent
			{
				OriginToken = input.Token
			});
			return new GrantWithRefreshTokenUseCaseOutput
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
				AuthType = "refresh_token",
				Data = new Dictionary<string, string>
				{
					{ "refresh_token", input.Token }
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