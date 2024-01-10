using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface IUserApi
{
	[Get("/api/user")]
	Task<IApiResponse<List<UserItemDto>>> SearchAsync([Query] UserCriteria criteria, int page, int size, CancellationToken cancellationToken = default);

	[Get("/api/user/count")]
	Task<IApiResponse<int>> CountAsync([Query] UserCriteria criteria, CancellationToken cancellationToken = default);

	[Get("/api/user/{id}")]
	Task<IApiResponse<UserDetailDto>> GetAsync(int id, CancellationToken cancellationToken = default);

	[Post("/api/user")]
	Task<IApiResponse> CreateAsync([Body] UserCreateDto data, CancellationToken cancellationToken = default);

	[Put("/api/user/{id}")]
	Task<IApiResponse> UpdateAsync(int id, [Body] UserUpdateDto data, CancellationToken cancellationToken = default);

	[Delete("/api/user/{id}")]
	Task<IApiResponse> DeleteAsync(int id, CancellationToken cancellationToken = default);
}