using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace Nerosoft.Starfish.Webapp.Rest;

internal class AuthorizationHandler : DelegatingHandler
{
	private readonly ILocalStorageService _storageService;

	public AuthorizationHandler(ILocalStorageService storageService)
	{
		_storageService = storageService;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var token = await _storageService.GetItemAsStringAsync(Constants.LocalStorage.AccessToken, cancellationToken);
		if (!string.IsNullOrWhiteSpace(token))
		{
			request.Headers!.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}

		return await base.SendAsync(request, cancellationToken);
	}
}