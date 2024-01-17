namespace Nerosoft.Starfish.Client;

internal class HttpConfigurationClient : IConfigurationClient
{
	private readonly HttpClient _httpClient = new();

	public HttpConfigurationClient(Uri host, string team, string app, string secret, string env)
	{
		_httpClient.BaseAddress = host;
		_httpClient.DefaultRequestHeaders.Add(Constants.RequestHeaders.Team, team);
		_httpClient.DefaultRequestHeaders.Add(Constants.RequestHeaders.App, app);
		_httpClient.DefaultRequestHeaders.Add(Constants.RequestHeaders.Secret, secret);
		_httpClient.DefaultRequestHeaders.Add(Constants.RequestHeaders.Env, env);
	}

	public async Task GetConfigurationAsync(Action<byte[], int> dataAction, CancellationToken cancellationToken = default)
	{
		var attempts = 0;
		RUN:
		try
		{
			attempts++;
			using var request = new HttpRequestMessage(HttpMethod.Get, "es");

			var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

			await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
			var bytes = new byte[64 * 1024 * 1024];
			int length;
			while ((length = await stream.ReadAsync(bytes, cancellationToken)) > 0)
			{
				dataAction(bytes, length);

#if __MOBILE__
				goto END;
#endif
			}
#if __MOBILE__
			END:
			await Task.CompletedTask;
#endif
		}
		catch (HttpRequestException exception)
		{
			switch (exception.StatusCode)
			{
				case System.Net.HttpStatusCode.RequestTimeout:
				case System.Net.HttpStatusCode.ServiceUnavailable:
				case System.Net.HttpStatusCode.GatewayTimeout:
				case System.Net.HttpStatusCode.BadGateway:
					if (attempts < 10)
					{
						await Task.Delay(2000, cancellationToken);
						goto RUN;
					}

					break;
			}

			throw;
		}
		catch (Exception exception)
		{
			switch (exception)
			{
				case TimeoutException:
				case TaskCanceledException:
				case OperationCanceledException:
					if (attempts < 10)
					{
						await Task.Delay(2000, cancellationToken);
						goto RUN;
					}

					break;
			}

			throw;
		}
	}
}