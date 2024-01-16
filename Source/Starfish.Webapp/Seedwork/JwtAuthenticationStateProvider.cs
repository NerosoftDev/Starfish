using Blazored.LocalStorage;
using IdentityModel;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using CommunityToolkit.Mvvm.Messaging;

namespace Nerosoft.Starfish.Webapp;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
	private readonly ILocalStorageService _storageService;

	public JwtAuthenticationStateProvider(ILocalStorageService storageService)
	{
		_storageService = storageService;
		if (!WeakReferenceMessenger.Default.IsRegistered<JwtAuthenticationStateProvider>(this))
		{
			WeakReferenceMessenger.Default.Register<AuthenticationStateMessage>(this, OnReceiveMessage);
		}
	}

	private async void OnReceiveMessage(object recipient, AuthenticationStateMessage message)
	{
		switch (message.Type)
		{
			case "logout":
				await _storageService.RemoveItemsAsync([Constants.LocalStorage.AccessToken, Constants.LocalStorage.RefreshToken]);
				break;
			case "login":
				await _storageService.SetItemAsStringAsync(Constants.LocalStorage.AccessToken, message.AccessToken);
				await _storageService.SetItemAsStringAsync(Constants.LocalStorage.RefreshToken, message.RefreshToken);
				break;
		}

		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		ClaimsIdentity identity;

		var token = await _storageService.GetItemAsStringAsync(Constants.LocalStorage.AccessToken);
		var jwt = TokenHelper.Resolve(token);
		if (jwt != null && jwt.ValidTo > DateTime.UtcNow)
		{
			identity = new ClaimsIdentity(jwt.Claims, "jwt", JwtClaimTypes.Name, JwtClaimTypes.Role);
		}
		else
		{
			identity = new ClaimsIdentity();
		}

		return new AuthenticationState(new ClaimsPrincipal(identity));
	}

	~JwtAuthenticationStateProvider()
	{
		WeakReferenceMessenger.Default.UnregisterAll(this);
	}
}

internal class AuthenticationStateMessage
{
	public AuthenticationStateMessage(string type)
	{
		Type = type;
	}

	public string Type { get; }

	public string AccessToken { get; init; }

	public string RefreshToken { get; init; }
}