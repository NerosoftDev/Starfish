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
		context.Services.AddScoped<IGrantWithPasswordUseCase, GrantWithPasswordUseCase>();
		context.Services.AddScoped<IGrantWithRefreshTokenUseCase, GrantWithRefreshTokenUseCase>();
	}
}