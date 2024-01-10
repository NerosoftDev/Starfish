using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
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
				Description = Resources.IDS_DICTIONARY_ENVIRONMENT_DEV
			},
			new()
			{
				Name = "SIT",
				Description = Resources.IDS_DICTIONARY_ENVIRONMENT_SIT
			},
			new()
			{
				Name = "UAT",
				Description = Resources.IDS_DICTIONARY_ENVIRONMENT_UAT
			},
			new()
			{
				Name = "PET",
				Description = Resources.IDS_DICTIONARY_ENVIRONMENT_PET
			},
			new()
			{
				Name = "SIM",
				Description = Resources.IDS_DICTIONARY_ENVIRONMENT_SIM
			},
			new()
			{
				Name = "PRD",
				Description = Resources.IDS_DICTIONARY_ENVIRONMENT_PRD
			}
		};
		return Task.FromResult(items);
	}

	/// <inheritdoc />
	public Task<List<DictionaryItemDto>> GetDatabaseTypeItemsAsync(CancellationToken cancellationToken = default)
	{
		var items = new List<DictionaryItemDto>
		{
			new()
			{
				Name = "mssql/sqlserver",
				Description = "Microsoft SQL Server"
			},
			new()
			{
				Name = "mysql",
				Description = "MySQL"
			},
			new()
			{
				Name = "postgresql/postgre/pg/pgsql/postgres",
				Description = "PostgreSQL"
			},
			new()
			{
				Name = "sqlite",
				Description = "SQLite"
			},
			new()
			{
				Name = "mongodb/mongo",
				Description = "MongoDb"
			}
		};
		return Task.FromResult(items);
	}

	/// <inheritdoc />
	public Task<List<DictionaryItemDto>> GetSettingItemTypeItemsAsync(CancellationToken cancellationToken = default)
	{
		var items = new List<DictionaryItemDto>
		{
			new()
			{
				Name = nameof(SettingItemType.Root),
				Description = Resources.IDS_ENUM_SETTING_ITEM_TYPE_ROOT
			},
			new()
			{
				Name = nameof(SettingItemType.Array),
				Description = Resources.IDS_ENUM_SETTING_ITEM_TYPE_ARRAY
			},
			new()
			{
				Name = nameof(SettingItemType.Object),
				Description = Resources.IDS_ENUM_SETTING_ITEM_TYPE_OBJECT
			},
			new()
			{
				Name = nameof(SettingItemType.String),
				Description = Resources.IDS_ENUM_SETTING_ITEM_TYPE_STRING
			},
			new()
			{
				Name = nameof(SettingItemType.Number),
				Description = Resources.IDS_ENUM_SETTING_ITEM_TYPE_NUMBER
			},
			new()
			{
				Name = nameof(SettingItemType.Boolean),
				Description = Resources.IDS_ENUM_SETTING_ITEM_TYPE_BOOLEAN
			}
		};
		return Task.FromResult(items);
	}
}