using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 仓储模块上下文
/// </summary>
public class RepositoryModule : ModuleContextBase
{
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
		var databaseType = Configuration.GetValue<string>("DatabaseType");

		context.Services.AddDbContextFactory<DataContext>(options =>
		{
			switch (databaseType)
			{
				case "mysql":
					options.UseMySql(connection, ServerVersion.AutoDetect(connection), builder =>
					{
						builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(2), null);
						builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
					});
					break;
				case "postgresql":
				case "postgre":
				case "pg":
				case "pgsql":
				case "postgres":
					options.UseNpgsql(connection, builder =>
					{
						builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(2), null);
						builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
					});
					break;
				case "mssql":
				case "sqlserver":
					options.UseSqlServer(connection, builder =>
					{
						builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(2), null);
						builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
					});
					break;
				case "sqlite":
					options.UseSqlite(connection, builder =>
					{
						builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
					});
					break;
				case "memory":
					options.UseInMemoryDatabase("Starfish");
					break;
				case "mongodb":
					options.UseMongoDB(connection, "");
					break;
				default:
					throw new NotSupportedException($"不支持的数据库类型：{databaseType}");
			}
		});

		context.Services.AddScoped<IUserRepository, UserRepository>();
	}
}