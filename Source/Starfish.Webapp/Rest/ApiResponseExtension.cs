using Newtonsoft.Json;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal static class ApiResponseExtension
{
	public static TContent EnsureSuccess<TContent>(this IApiResponse<TContent> response)
	{
		if (response.IsSuccessStatusCode)
		{
			return response.Content ?? default;
		}

		throw response.Error!;
	}

	public static void EnsureSuccess(this IApiResponse response)
	{
		if (!response.IsSuccessStatusCode)
		{
			throw response.Error!;
		}
	}

	public static ApiResponseDetail GetDetail(this ApiException exception)
	{
		var content = exception.Content;

		if (content == null)
		{
			return new ApiResponseDetail { Message = exception.Message, StatusCode = (int)exception.StatusCode };
		}

		var response = JsonConvert.DeserializeObject<ApiResponseDetail>(content);

		return response;
	}

	public static Task EnsureSuccess(this Task<IApiResponse> task, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(task, nameof(task));
		return task.ContinueWith(t =>
		{
			t.WaitAndUnwrapException(cancellationToken);
			t.Result.EnsureSuccess();
		}, cancellationToken);
	}

	public static Task<TResult> EnsureSuccess<TResult>(this Task<IApiResponse<TResult>> task, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(task, nameof(task));
		return task.ContinueWith(t =>
		{
			t.WaitAndUnwrapException(cancellationToken);
			return t.Result.EnsureSuccess();
		}, cancellationToken);
	}

	public static Task EnsureSuccess<TResult>(this Task<IApiResponse<TResult>> task, Action<TResult> next, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(task, nameof(task));
		return task.ContinueWith(t =>
		{
			t.WaitAndUnwrapException(cancellationToken);
			var result = t.Result.EnsureSuccess();
			next?.Invoke(result);
		}, cancellationToken);
	}
}