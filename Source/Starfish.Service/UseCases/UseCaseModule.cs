using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Modularity;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 用例模块
/// </summary>
public class UseCaseModule : ModuleContextBase
{
	/// <inheritdoc />
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		context.Services.AddSingleton<IdentityCommonComponent>();
		context.Services
		       .AddScoped<IGrantWithPasswordUseCase, GrantWithPasswordUseCase>()
		       .AddScoped<IGrantWithRefreshTokenUseCase, GrantWithRefreshTokenUseCase>()
		       .AddScoped<ILogsCountUseCase, LogsCountUseCase>()
		       .AddScoped<ILogsSearchUseCase, LogsSearchUseCase>()
		       .AddScoped<IAppInfoSearchUseCase, AppInfoSearchUseCase>()
		       .AddScoped<IAppInfoCountUseCase, AppInfoCountUseCase>()
		       .AddScoped<IAppInfoDetailUseCase, AppInfoDetailUseCase>()
		       .AddScoped<IAppInfoCreateUseCase, AppInfoCreateUseCase>()
		       .AddScoped<IAppInfoUpdateUseCase, AppInfoUpdateUseCase>()
		       .AddScoped<IAppInfoDeleteUseCase, AppInfoDeleteUseCase>()
		       .AddScoped<IChangeAppInfoStatusUseCase, ChangeAppInfoStatusUseCase>()
		       .AddScoped<IAppInfoAuthorizeUseCase, AppInfoAuthorizeUseCase>();
	}
}