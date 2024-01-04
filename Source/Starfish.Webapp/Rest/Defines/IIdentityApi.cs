using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface IIdentityApi
{
	[Post("/api/identity/grant")]
	Task<IApiResponse<AuthResponseDto>> GrantTokenAsync([Body] AuthRequestDto request, CancellationToken cancellationToken = default);
	
	[Post("/api/identity/refresh")]
	Task<IApiResponse<AuthResponseDto>> RefreshTokenAsync(string token, CancellationToken cancellationToken = default);
}