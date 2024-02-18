using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface IConfigurationApi
{
	[Get("/api/configuration")]
	Task<IApiResponse<List<ConfigurationDto>>> QueryAsync([Query] ConfigurationCriteria criteria, int skip = Constants.Query.Skip, int count = Constants.Query.Count, CancellationToken cancellationToken = default);

	[Post("/api/configuration")]
	Task<IApiResponse> CreateAsync(string teamId, [Body] ConfigurationEditDto data, CancellationToken cancellationToken = default);

	[Get("/api/configuration/count")]
	Task<IApiResponse<int>> CountAsync([Query] ConfigurationCriteria criteria, CancellationToken cancellationToken = default);

	[Get("/api/configuration/{id}")]
	Task<IApiResponse<ConfigurationDto>> GetAsync(string id, CancellationToken cancellationToken = default);

	[Put("/api/configuration/{id}")]
	Task<IApiResponse> UpdateAsync(string id, [Body] ConfigurationEditDto data, CancellationToken cancellationToken = default);

	[Delete("/api/configuration/{id}")]
	Task<IApiResponse> DeleteAsync(string id, CancellationToken cancellationToken = default);

	[Post("/api/configuration/{id}/publish")]
	Task<IApiResponse> PublishAsync(string id, [Body] ConfigurationPublishDto data, CancellationToken cancellationToken = default);

	[Post("/api/configuration/{id}/redis")]
	Task<IApiResponse> PushRedisAsync(string id, [Body] PushRedisRequestDto data, CancellationToken cancellationToken = default);

	[Get("/api/configuration/{id}/item")]
	[Headers("x-format: application/json")]
	Task<IApiResponse<List<ConfigurationItemDto>>> GetItemListAsync(string id, int skip = Constants.Query.Skip, int count = Constants.Query.Count, CancellationToken cancellationToken = default);

	[Get("/api/configuration/{id}/item")]
	Task<IApiResponse<string>> GetItemsAsync(string id, [Header("x-format")] string format, CancellationToken cancellationToken = default);

	[Get("/api/configuration/{id}/item/count")]
	Task<IApiResponse<int>> GetItemCountAsync(string id, CancellationToken cancellationToken = default);

	[Put("/api/configuration/{id}/item/{key}")]
	Task<IApiResponse> UpdateValueAsync(string id, string key, [Body] string data, CancellationToken cancellationToken = default);

	[Put("/api/configuration/{id}/item")]
	Task<IApiResponse> UpdateItemsAsync(string id, [Header("x-format")] string format, [Body] string data, CancellationToken cancellationToken = default);
}