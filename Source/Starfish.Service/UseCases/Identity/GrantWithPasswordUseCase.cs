using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Core;
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
	public string UserName { get; set; }

	/// <summary>
	/// 密码
	/// </summary>
	public string Password { get; set; }
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
	private readonly IUserRepository _repository;
	private readonly IdentityCommonComponent _component;

	/// <summary>
	/// 初始化<see cref="GrantWithPasswordUseCase"/>.
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="component"></param>
	/// <param name="presenter"></param>
	public GrantWithPasswordUseCase(IUserRepository repository, IdentityCommonComponent component, IUseCasePresenter<GrantWithPasswordUseCaseOutput> presenter)
	{
		_repository = repository;
		_component = component;
		Presenter = presenter;
	}

	/// <inheritdoc />
	public async Task<GrantWithPasswordUseCaseOutput> ExecuteAsync(GrantWithPasswordUseCaseInput input, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(input.UserName) || string.IsNullOrWhiteSpace(input.Password))
		{
			throw new BadRequestException(Resources.IDS_USERNAME_OR_PASSWORD_IS_INVALID);
		}

		var user = await _repository.FindByUserNameAsync(input.UserName, false, cancellationToken);
		if (user == null)
		{
			throw new BadRequestException(Resources.IDS_USERNAME_OR_PASSWORD_IS_INVALID);
		}

		var passwordHash = Cryptography.DES.Encrypt(input.Password, Encoding.UTF8.GetBytes(user.PasswordSalt));

		if (string.Equals(user.PasswordHash, passwordHash))
		{
			throw new AuthenticationException(Resources.IDS_USERNAME_OR_PASSWORD_IS_INVALID);
		}

		if (user.LockoutEnd > DateTime.UtcNow)
		{
			throw new AuthenticationException(Resources.IDS_USER_LOCKOUT);
		}

		var roles = user.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
		var (token, issuesAt, expiresAt) = _component.GenerateAccessToken(user.Id, user.UserName, roles);
		return new GrantWithPasswordUseCaseOutput
		{
			Token = token,
			IssuesAt = issuesAt,
			ExpiresAt = expiresAt
		};
	}

	/// <inheritdoc />
	public IUseCasePresenter<GrantWithPasswordUseCaseOutput> Presenter { get; }
}