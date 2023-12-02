using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Bus.RabbitMq;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Application;
using Nerosoft.Euonia.Bus.InMemory;
using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 领域模块上下文
/// </summary>
public sealed class DomainServiceModule : ModuleContextBase
{
	/// <inheritdoc/>
	public override void AheadConfigureServices(ServiceConfigurationContext context)
	{
		Configure<UnitOfWorkOptions>(options =>
		{
			options.IsTransactional = false;
		});
	}

	/// <inheritdoc/>
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		context.Services.AddContextProvider();
		context.Services.AddUnitOfWork();

		var connection = Configuration.GetConnectionString("DefaultConnection")!;
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

		context.Services.AddServiceBus(config =>
		{
			config.RegisterHandlers(typeof(ApplicationServiceModule).Assembly);
			var provider = Configuration.GetValue<string>("ServiceBus:Provider")?.ToLower();
			switch (provider)
			{
				case null:
				case "":
				case "inmemory":
					config.UseInMemory(options =>
					{
						options.MessengerReference = Configuration.GetValue<MessengerReferenceType>("ServiceBus:InMemory:MessengerReference");
						options.MultipleSubscriberInstance = Configuration.GetValue<bool>("ServiceBus:InMemory:MultipleSubscriberInstance");
					});
					break;
				case "rabbitmq":
					config.UseRabbitMq(options =>
					{
						options.Connection = Configuration.GetValue<string>("ServiceBus:RabbitMq:Connection");
						options.ExchangeName = Configuration.GetValue<string>("ServiceBus:RabbitMq:ExchangeName");
						options.ExchangeType = Configuration.GetValue<string>("ServiceBus:RabbitMq:ExchangeType");
						options.QueueName = Configuration.GetValue<string>("ServiceBus:RabbitMq:QueueName");
						options.TopicName = Configuration.GetValue<string>("ServiceBus:RabbitMq:TopicName");
					});
					break;
				default:
					throw new NotSupportedException($"不支持的消息总线提供程序：{provider}");
			}
		});
	}
}
