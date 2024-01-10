using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 仓储模块上下文
/// </summary>
public class RepositoryModule : ModuleContextBase
{
	private static readonly Dictionary<string, DatabaseType> _databaseTypeAlias = new()
	{
		{ "mssql", DatabaseType.SqlServer },
		{ "sqlserver", DatabaseType.SqlServer },
		{ "mysql", DatabaseType.MySql },
		{ "postgresql", DatabaseType.PostgreSql },
		{ "postgre", DatabaseType.PostgreSql },
		{ "pg", DatabaseType.PostgreSql },
		{ "pgsql", DatabaseType.PostgreSql },
		{ "postgres", DatabaseType.PostgreSql },
		{ "sqlite", DatabaseType.Sqlite },
		{ "mongodb", DatabaseType.MongoDb },
		{ "mongo", DatabaseType.MongoDb },
		{ "memory", DatabaseType.InMemory },
		{ "inmemory", DatabaseType.InMemory },
		{ "in-memory", DatabaseType.InMemory }
	};

	/// <inheritdoc/>
	public override void AheadConfigureServices(ServiceConfigurationContext context)
	{
		Configure<UnitOfWorkOptions>(options =>
		{
			options.IsTransactional = false;
		});
	}

	/// <inheritdoc />
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		context.Services.AddContextProvider();
		context.Services.AddUnitOfWork();

		var connection = Configuration.GetConnectionString("Default")!;
		var databaseTypeValue = Configuration.GetValue<string>("DatabaseType")?.ToLowerInvariant();

		var databaseType = _databaseTypeAlias.GetValueOrDefault(databaseTypeValue, DatabaseType.InMemory);

		switch (databaseType)
		{
			case DatabaseType.Sqlite:
				context.Services.TryAddSingleton<IModelBuilder, SqliteModelBuilder>();
				break;
			case DatabaseType.InMemory:
				context.Services.TryAddSingleton<IModelBuilder, InMemoryModelBuilder>();
				break;
			case DatabaseType.MongoDb:
				context.Services.TryAddSingleton<IModelBuilder, MongoModelBuilder>();
				break;
			case DatabaseType.MySql:
				context.Services.TryAddSingleton<IModelBuilder, MysqlModelBuilder>();
				break;
			case DatabaseType.SqlServer:
				context.Services.TryAddSingleton<IModelBuilder, MssqlModelBuilder>();
				break;
			case DatabaseType.PostgreSql:
				context.Services.TryAddSingleton<IModelBuilder, PgsqlModelBuilder>();
				break;
			default:
				throw new NotSupportedException(string.Format(Resources.IDS_ERROR_UNSUPPORTED_DATABASE_PROVIDER, databaseTypeValue));
		}

		context.Services.AddDbContextFactory<DataContext>(options =>
		{
			switch (databaseType)
			{
				case DatabaseType.MySql:
					options.UseMySql(connection, ServerVersion.AutoDetect(connection), builder =>
					{
						builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(2), null);
						builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
					});
					break;
				case DatabaseType.PostgreSql:
					options.UseNpgsql(connection, builder =>
					{
						builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(2), null);
						builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
					});
					break;
				case DatabaseType.SqlServer:
					options.UseSqlServer(connection, builder =>
					{
						builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(2), null);
						builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
					});
					break;
				case DatabaseType.Sqlite:
					options.UseSqlite(connection, builder =>
					{
						builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
					});
					break;
				case DatabaseType.InMemory:
					options.UseInMemoryDatabase("Starfish");
					break;
				case DatabaseType.MongoDb:
					options.UseMongoDB(connection, "");
					break;
				default:
					throw new NotSupportedException(string.Format(Resources.IDS_ERROR_UNSUPPORTED_DATABASE_PROVIDER, databaseTypeValue));
			}
		});

		context.Services
		       .AddScoped<IUserRepository, UserRepository>()
		       .AddScoped<ITokenRepository, TokenRepository>()
		       .AddScoped<IOperateLogRepository, OperateLogRepository>()
		       .AddScoped<ITeamRepository, TeamRepository>()
		       .AddScoped<IAppInfoRepository, AppInfoRepository>()
		       .AddScoped<ISettingRepository, SettingRepository>()
		       .AddScoped<ISettingRevisionRepository, SettingRevisionRepository>()
		       .AddScoped<ISettingArchiveRepository, SettingArchiveRepository>();
	}
}