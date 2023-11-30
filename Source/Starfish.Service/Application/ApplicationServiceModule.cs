using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Bus.InMemory;
using Nerosoft.Euonia.Caching;
using Nerosoft.Euonia.Caching.Memory;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Euonia.Modularity;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用服务模块上下文
/// </summary>
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
}
