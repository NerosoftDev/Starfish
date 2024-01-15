using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface ISettingApi
{
	[Get("/api/apps/{id}/setting/{environment}/item")]
	[Headers("x-format: application/json")]
	Task<IApiResponse<List<SettingItemDto>>> GetItemListAsync(long id, string environment, int page, int size, CancellationToken cancellationToken = default);

	[Get("/api/apps/{id}/setting/{environment}/item/count")]
	Task<IApiResponse<int>> GetItemCountAsync(long id, string environment, CancellationToken cancellationToken = default);

	[Get("/api/apps/{id}/setting/{environment}/item")]
	Task<IApiResponse<string>> GetItemsAsync(long id, string environment, [Header("x-format")] string format, CancellationToken cancellationToken = default);

	[Get("/api/apps/{id}/setting/{environment}/detail")]
	Task<IApiResponse<SettingDetailDto>> GetAsync(long id, string environment, CancellationToken cancellationToken = default);

	[Post("/api/apps/{id}/setting/{environment}")]
	Task<IApiResponse> CreateAsync(long id, string environment, [Header("x-format")] string format, [Body] SettingEditDto data, CancellationToken cancellationToken = default);

	[Put("/api/apps/{id}/setting/{environment}")]
	Task<IApiResponse> UpdateAsync(long id, string environment, [Header("x-format")] string format, [Body] SettingEditDto data, CancellationToken cancellationToken = default);

	[Delete("/api/apps/{id}/setting/{environment}")]
	Task<IApiResponse> DeleteAsync(long id, string environment, CancellationToken cancellationToken = default);

	[Put("/api/apps/{id}/setting/{environment}/item/{key}")]
	Task<IApiResponse> UpdateItemValueAsync(long id, string environment, string key, [Body] string value, CancellationToken cancellationToken = default);

	[Post("/api/apps/{id}/setting/{environment}/item/publish")]
	Task<IApiResponse> PublishAsync(long id, string environment, [Body] SettingPublishDto data, CancellationToken cancellationToken = default);
}