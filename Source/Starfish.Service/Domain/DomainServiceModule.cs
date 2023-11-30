using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Bus.RabbitMq;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Application;
using Nerosoft.Euonia.Bus.InMemory;

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
