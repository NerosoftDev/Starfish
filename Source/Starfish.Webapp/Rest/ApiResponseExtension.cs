﻿using Newtonsoft.Json;
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
}