using System.Security.Authentication;
using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Core;
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
	private readonly IdentityCommonComponent _component;

	/// <summary>
	/// 初始化<see cref="GrantWithRefreshTokenUseCase"/>.
	/// </summary>
	/// <param name="provider"></param>
	/// <param name="component"></param>
	public GrantWithRefreshTokenUseCase(IServiceProvider provider, IdentityCommonComponent component)
	{
		_provider = provider;
		_component = component;
	}

	private IUserRepository UserRepository => _provider.GetService<IUserRepository>();
	private ITokenRepository TokenRepository => _provider.GetService<ITokenRepository>();

	/// <inheritdoc />
	public async Task<GrantWithRefreshTokenUseCaseOutput> ExecuteAsync(GrantWithRefreshTokenUseCaseInput input, CancellationToken cancellationToken = new CancellationToken())
	{
		if (string.IsNullOrWhiteSpace(input.Token))
		{
			throw new BadRequestException("refresh_token is required");
		}

		var key = input.Token.ToSha256();

		var token = await TokenRepository.GetAsync(t => t.Key == key, false, cancellationToken);

		if (token == null)
		{
			throw new BadRequestException(Resources.IDS_REFRESH_TOKEN_IS_INVALID);
		}

		if (token.Expires < DateTime.UtcNow)
		{
			throw new BadRequestException(Resources.IDS_REFRESH_TOKEN_EXPIRED);
		}

		var user = await UserRepository.GetAsync(int.Parse(token.Subject), false, cancellationToken);

		if (user == null)
		{
			throw new BadRequestException(Resources.IDS_REFRESH_TOKEN_IS_INVALID);
		}

		if (user.LockoutEnd > DateTime.UtcNow)
		{
			throw new AuthenticationException(Resources.IDS_USER_LOCKOUT);
		}

		var roles = user.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
		var (accessToken, issuesAt, expiresAt) = _component.GenerateAccessToken(user.Id, user.UserName, roles);
		return new GrantWithRefreshTokenUseCaseOutput
		{
			UserId = user.Id,
			Token = accessToken,
			IssuesAt = issuesAt,
			ExpiresAt = expiresAt
		};
	}

	/// <inheritdoc />
	public IUseCasePresenter<GrantWithRefreshTokenUseCaseOutput> Presenter { get; }
}