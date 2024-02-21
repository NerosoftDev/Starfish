using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal static class ServiceCollectionExtensions
{
	private const string HTTP_CLIENT_NAME = "starfish";

	private static readonly RefitSettings _refitSettings = new()
	{
		ContentSerializer = new NewtonsoftJsonContentSerializer()
		//ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
		//{
		//	NumberHandling = JsonNumberHandling.AllowReadingFromString,
		//	Converters =
		//	{
		//		new DateTimeConverter(),
		//		new NullableDateTimeConverter(),
		//		new JsonStringEnumConverter()
		//	}
		//})
	};

	public static IServiceCollection AddHttpClientApi(this IServiceCollection services, Action<RestServiceOptions> config)
	{
		services.Configure(config);
		services.AddTransient<LoggingHandler>();
		services.AddTransient<SlowRequestHandler>();
		services.AddTransient<AuthorizationHandler>();

		services.AddTransient(provider => provider.GetRestService<IIdentityApi>(HTTP_CLIENT_NAME))
		        .AddTransient(provider => provider.GetRestService<ILogsApi>(HTTP_CLIENT_NAME))
		        .AddTransient(provider => provider.GetRestService<IUserApi>(HTTP_CLIENT_NAME))
		        .AddTransient(provider => provider.GetRestService<ITeamApi>(HTTP_CLIENT_NAME))
		        .AddTransient(provider => provider.GetRestService<IAdministratorApi>(HTTP_CLIENT_NAME))
		        .AddTransient(provider => provider.GetRestService<IConfigurationApi>(HTTP_CLIENT_NAME));

		services.AddHttpClient(HTTP_CLIENT_NAME, (provider, client) =>
		        {
			        var options = provider.GetService<IOptions<RestServiceOptions>>().Value;
			        client.BaseAddress = new Uri(options.BaseUrl);
			        client.Timeout = options.Timeout;
		        })
		        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
		        .AddHttpMessageHandler<AuthorizationHandler>()
		        .AddHttpMessageHandler<LoggingHandler>()
		        .AddHttpMessageHandler<SlowRequestHandler>();
		// .AddHttpMessageHandler(provider =>
		// {
		//  var handler = new ProgressMessageHandler();
		//  handler.HttpSendProgress += (sender, args) =>
		//  {
		//   Console.WriteLine($"Send progress: {args.ProgressPercentage}");
		//  };
		//  return handler;
		// });

		return services;
	}

	private static HttpClient GetHttpClient(this IServiceProvider provider, string name)
	{
		var factory = provider.GetService<IHttpClientFactory>();
		var client = factory.CreateClient(name);
		return client;
	}

	private static TService GetRestService<TService>(this IServiceProvider provider, string name)
	{
		var client = provider.GetHttpClient(name);
		return RestService.For<TService>(client, _refitSettings);
	}

	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
	{
		return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(15));
	}

	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
	{
		return HttpPolicyExtensions.HandleTransientHttpError()
		                           .Or<TimeoutRejectedException>()
		                           .Or<SocketException>()
		                           .OrResult(response => response.StatusCode == HttpStatusCode.NotFound)
		                           .OrResult(response => response.StatusCode == HttpStatusCode.ServiceUnavailable)
		                           .WaitAndRetryAsync(5,
			                           retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
	}
}