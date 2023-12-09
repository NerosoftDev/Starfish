﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Bus.InMemory;
using Nerosoft.Euonia.Bus.RabbitMq;
using Nerosoft.Euonia.Caching;
using Nerosoft.Euonia.Caching.Memory;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用服务模块上下文
/// </summary>
[DependsOn(typeof(RepositoryModule), typeof(DomainServiceModule))]
public sealed class ApplicationServiceModule : ModuleContextBase
{
	/// <inheritdoc/>
	public override void AheadConfigureServices(ServiceConfigurationContext context)
	{
		Configure<AutomapperOptions>(options =>
		{
		});
	}

	/// <inheritdoc/>
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		context.Services.Register<ApplicationServiceContext>();

		ConfigureCachingServices(context.Services);

		ConfigureBusServices(context.Services);
	}

	private void ConfigureCachingServices(IServiceCollection services)
	{
		var provider = Configuration.GetValue<string>("Cache:Provider")?.ToLowerInvariant();
		switch (provider)
		{
			case null:
			case "":
			case "memory":
				services.AddSingleton<ICacheService, MemoryCacheService>();
				break;
			case "redis":
				services.AddSingleton<ICacheService, RedisCacheService>();
				break;
			default:
				throw new NotSupportedException($"不支持的缓存提供程序：{provider}");
		}

		services.AddDefaultCacheManager<ApplicationServiceContext>();
	}

	private void ConfigureBusServices(IServiceCollection services)
	{
		services.AddServiceBus(config =>
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