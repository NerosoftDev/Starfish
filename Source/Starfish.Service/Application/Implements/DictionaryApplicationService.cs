using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 字典应用服务
/// </summary>
public class DictionaryApplicationService : BaseApplicationService, IDictionaryApplicationService
{
	/// <inheritdoc />
	public Task<List<DictionaryItemDto>> GetRoleItemsAsync(CancellationToken cancellationToken = default)
	{
		var items = new List<DictionaryItemDto>
		{
			new()
			{
				Name = "SA",
				Description = Resources.IDS_DICTIONARY_NAME_SA
			},
			new()
			{
				Name = "RW",
				Description = "Read and write"
			},
			new()
			{
				Name = "RO",
				Description = "Read only"
			}
		};
		return Task.FromResult(items);
	}

	/// <inheritdoc />
	public Task<List<DictionaryItemDto>> GetEnvironmentItemsAsync(CancellationToken cancellationToken = default)
	{
		var items = new List<DictionaryItemDto>
		{
			new()
			{
				Name = "DEV",
				Description = "Development"
			},
			new()
			{
				Name = "SIT",
				Description = "System Integration Testing"
			},
			new()
			{
				Name = "UAT",
				Description = "User Acceptance Testing"
			},
			new()
			{
				Name = "PET",
				Description = "Performance Evaluation Testing"
			},
			new()
			{
				Name = "SIM",
				Description = "Simulation Testing"
			},
			new()
			{
				Name = "PRD",
				Description = "Production"
			}
		};
		return Task.FromResult(items);
	}
}