using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Nerosoft.Starfish.Client;
using Nerosoft.Starfish.Webapp.Rest;

namespace Nerosoft.Starfish.Webapp;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebAssemblyHostBuilder.CreateDefault(args);

		builder.Configuration.AddStarfish(ConfigurationClientOptions.Load(builder.Configuration));
		
		builder.RootComponents.Add<App>("#app");
		builder.RootComponents.Add<HeadOutlet>("head::after");

		builder.Services.AddFluentUIComponents();

		builder.Services.AddOptions();
		builder.Services.AddAuthorizationCore();
		builder.Services
			   .AddScoped<JwtAuthenticationStateProvider>()
			   .AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<JwtAuthenticationStateProvider>())
			   .AddCascadingAuthenticationState()
			   .AddBlazoredLocalStorageAsSingleton()
			   .AddHttpClientApi(options =>
			   {
				   var baseUrl = builder.Configuration.GetValue<string>("Api:BaseUrl");
				   var timeout = builder.Configuration.GetValue<int>("Api:Timeout");
				   if (string.IsNullOrEmpty(baseUrl))
				   {
					   baseUrl = builder.HostEnvironment.BaseAddress;
				   }

				   options.BaseUrl = baseUrl;
				   options.Timeout = TimeSpan.FromMilliseconds(timeout);
			   });

		var host = builder.Build();
		Singleton<HostAccessor>.Get(() => new HostAccessor
		{
			ServiceProvider = host.Services,
			Configuration = host.Configuration
		});
		await host.RunAsync();
	}
}