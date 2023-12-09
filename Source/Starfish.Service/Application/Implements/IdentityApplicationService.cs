using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Domain;
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
		var events = new List<ApplicationEvent>();
		try
		{
			IdentityUseCaseOutput output = type switch
			{
				"password" => await LazyServiceProvider.GetService<IGrantWithPasswordUseCase>()
				                                       .ExecuteAsync(new GrantWithPasswordUseCaseInput
				                                       {
					                                       UserName = data.GetValueOrDefault("username"),
					                                       Password = data.GetValueOrDefault("password")
				                                       }, cancellationToken),
				"refresh_token" => await LazyServiceProvider.GetService<IGrantWithRefreshTokenUseCase>()
				                                            .ExecuteAsync(new GrantWithRefreshTokenUseCaseInput
				                                            {
					                                            Token = data.GetValueOrDefault("refresh_token")
				                                            }, cancellationToken),
				"otp" => throw new NotSupportedException(),
				_ => throw new NotSupportedException()
			};

			var refreshToken = Guid.NewGuid().ToString("N");

			foreach (var @event in events)
			{
				if (@event is UserAuthSucceedEvent e)
				{
					e.RefreshToken = refreshToken;
					e.TokenIssueTime = output.IssuesAt;
				}
			}

			return new AuthResponseDto
			{
				AccessToken = output.Token,
				TokenType = "Bearer",
				RefreshToken = refreshToken,
				ExpiresIn = new DateTimeOffset(output.ExpiresAt).ToUnixTimeSeconds()
			};
		}
		catch (Exception exception)
		{
			if (exception is AuthenticationException)
			{
				events.Add(new UserAuthFailedEvent
				{
					AuthType = type,
					Data = data,
					Error = exception.Message
				});
			}

			throw;
		}
		finally
		{
			foreach (var @event in events)
			{
				await Bus.PublishAsync(@event, cancellationToken);
			}
		}
	}
}