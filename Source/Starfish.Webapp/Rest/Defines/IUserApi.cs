﻿using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface IUserApi
{
	[Get("/api/user")]
	Task<IApiResponse<List<UserItemDto>>> SearchAsync([Query] UserCriteria criteria, int skip = Constants.Query.Skip, int count = Constants.Query.Count, CancellationToken cancellationToken = default);

	[Get("/api/user/count")]
	Task<IApiResponse<int>> CountAsync([Query] UserCriteria criteria, CancellationToken cancellationToken = default);

	[Get("/api/user/{id}")]
	Task<IApiResponse<UserDetailDto>> GetAsync(string id, CancellationToken cancellationToken = default);

	[Post("/api/user")]
	Task<IApiResponse> CreateAsync([Body] UserCreateDto data, CancellationToken cancellationToken = default);

	[Put("/api/user/{id}")]
	Task<IApiResponse> UpdateAsync(string id, [Body] UserUpdateDto data, CancellationToken cancellationToken = default);

	[Delete("/api/user/{id}")]
	Task<IApiResponse> DeleteAsync(string id, CancellationToken cancellationToken = default);

	[Put("/api/user/{id}/password")]
	Task<IApiResponse> ResetPassword(string id, [Body] ResetPasswordRequestDto data, CancellationToken cancellationToken = default);

	[Put("/api/user/password")]
	Task<IApiResponse> ChangePassword([Body] ChangePasswordRequestDto data, CancellationToken cancellationToken = default);
}