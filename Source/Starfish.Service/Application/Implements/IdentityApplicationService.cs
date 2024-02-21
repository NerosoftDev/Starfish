using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;
using Nerosoft.Starfish.UseCases;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 授权和认证应用服务实现
/// </summary>
// ReSharper disable once UnusedType.Global
public class IdentityApplicationService : BaseApplicationService, IIdentityApplicationService
{
	/// <inheritdoc />
	public async Task<AuthResponseDto> GrantAsync(string type, Dictionary<string, string> data, CancellationToken cancellationToken = default)
	{
		IdentityUseCaseOutput output = type switch
		{
			"password" => await LazyServiceProvider.GetService<IGrantWithPasswordUseCase>()
			                                       .ExecuteAsync(new GrantWithPasswordUseCaseInput(data.GetValueOrDefault("username"), data.GetValueOrDefault("password")), cancellationToken),
			"refresh_token" => await LazyServiceProvider.GetService<IGrantWithRefreshTokenUseCase>()
			                                            .ExecuteAsync(new GrantWithRefreshTokenUseCaseInput(data.GetValueOrDefault("refresh_token")), cancellationToken),
			"otp" => throw new NotSupportedException(),
			_ => throw new NotSupportedException()
		};

		return new AuthResponseDto
		{
			AccessToken = output.AccessToken,
			TokenType = "Bearer",
			RefreshToken = output.RefreshToken,
			ExpiresIn = new DateTimeOffset(output.ExpiresAt).ToUnixTimeSeconds()
		};
	}
}