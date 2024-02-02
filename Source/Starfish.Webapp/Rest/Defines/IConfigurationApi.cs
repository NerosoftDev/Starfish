using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface IConfigurationApi
{
	[Get("/api/apps/{id}/configuration/{environment}/item")]
	[Headers("x-format: application/json")]
	Task<IApiResponse<List<ConfigurationItemDto>>> GetItemListAsync(string id, string environment, int skip = Constants.Query.Skip, int count = Constants.Query.Count, CancellationToken cancellationToken = default);

	[Get("/api/apps/{id}/configuration/{environment}/item/count")]
	Task<IApiResponse<int>> GetItemCountAsync(string id, string environment, CancellationToken cancellationToken = default);

	[Get("/api/apps/{id}/configuration/{environment}/item")]
	Task<IApiResponse<string>> GetItemsAsync(string id, string environment, [Header("x-format")] string format, CancellationToken cancellationToken = default);

	[Get("/api/apps/{id}/configuration/{environment}/detail")]
	Task<IApiResponse<ConfigurationDetailDto>> GetAsync(string id, string environment, CancellationToken cancellationToken = default);

	[Post("/api/apps/{id}/configuration/{environment}")]
	Task<IApiResponse> CreateAsync(string id, string environment, [Header("x-format")] string format, [Body] ConfigurationEditDto data, CancellationToken cancellationToken = default);

	[Put("/api/apps/{id}/configuration/{environment}")]
	Task<IApiResponse> UpdateAsync(string id, string environment, [Header("x-format")] string format, [Body] ConfigurationEditDto data, CancellationToken cancellationToken = default);

	[Delete("/api/apps/{id}/configuration/{environment}")]
	Task<IApiResponse> DeleteAsync(string id, string environment, CancellationToken cancellationToken = default);

	[Put("/api/apps/{id}/configuration/{environment}/{key}")]
	Task<IApiResponse> UpdateItemValueAsync(string id, string environment, string key, [Body] ConfigurationValueUpdateDto data, CancellationToken cancellationToken = default);

	[Post("/api/apps/{id}/configuration/{environment}/publish")]
	Task<IApiResponse> PublishAsync(string id, string environment, [Body] ConfigurationPublishDto data, CancellationToken cancellationToken = default);

	[Get("/api/apps/{id}/configuration/{environment}/archive")]
	Task<IApiResponse<string>> GetArchivedAsync(string id, string environment, CancellationToken cancellationToken = default);

	[Post("/api/apps/{id}/configuration/{environment}/redis")]
	Task<IApiResponse> PushRedisAsync(string id, string environment, [Body] PushRedisRequestDto data);
}