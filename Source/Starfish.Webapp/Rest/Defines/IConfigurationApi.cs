using Nerosoft.Starfish.Transit;
using Refit;

namespace Nerosoft.Starfish.Webapp.Rest;

internal interface IConfigurationApi
{
	[Get("/api/apps/{id}/configuration/{environment}/item")]
	[Headers("x-format: application/json")]
	Task<IApiResponse<List<ConfigurationItemDto>>> GetItemListAsync(long id, string environment, int skip = Constants.Query.Skip, int count = Constants.Query.Count, CancellationToken cancellationToken = default);

	[Get("/api/apps/{id}/configuration/{environment}/item/count")]
	Task<IApiResponse<int>> GetItemCountAsync(long id, string environment, CancellationToken cancellationToken = default);

	[Get("/api/apps/{id}/configuration/{environment}/item")]
	Task<IApiResponse<string>> GetItemsAsync(long id, string environment, [Header("x-format")] string format, CancellationToken cancellationToken = default);

	[Get("/api/apps/{id}/configuration/{environment}/detail")]
	Task<IApiResponse<ConfigurationDetailDto>> GetAsync(long id, string environment, CancellationToken cancellationToken = default);

	[Post("/api/apps/{id}/configuration/{environment}")]
	Task<IApiResponse> CreateAsync(long id, string environment, [Header("x-format")] string format, [Body] ConfigurationEditDto data, CancellationToken cancellationToken = default);

	[Put("/api/apps/{id}/configuration/{environment}")]
	Task<IApiResponse> UpdateAsync(long id, string environment, [Header("x-format")] string format, [Body] ConfigurationEditDto data, CancellationToken cancellationToken = default);

	[Delete("/api/apps/{id}/configuration/{environment}")]
	Task<IApiResponse> DeleteAsync(long id, string environment, CancellationToken cancellationToken = default);

	[Put("/api/apps/{id}/configuration/{environment}/{key}")]
	Task<IApiResponse> UpdateItemValueAsync(long id, string environment, string key, [Body] ConfigurationValueUpdateDto data, CancellationToken cancellationToken = default);

	[Post("/api/apps/{id}/configuration/{environment}/publish")]
	Task<IApiResponse> PublishAsync(long id, string environment, [Body] ConfigurationPublishDto data, CancellationToken cancellationToken = default);

	[Get("/api/apps/{id}/configuration/{environment}/archive")]
	Task<IApiResponse<string>> GetArchivedAsync(long id, string environment, CancellationToken cancellationToken = default);

	[Post("/api/apps/{id}/configuration/{environment}/redis")]
	Task<IApiResponse> PushRedisAsync(long id, string environment, [Body] PushRedisRequestDto data);
}