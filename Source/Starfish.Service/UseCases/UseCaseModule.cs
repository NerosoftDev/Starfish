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
		var types = typeof(UseCaseModule).Assembly.GetTypes()
		                                 .Where(t => t.IsClass && t.Name.EndsWith("UseCase"));

		foreach (var type in types)
		{
			var interfaces = type.GetInterfaces()
			                     .Where(i => i.Name.EndsWith("UseCase"));

			foreach (var @interface in interfaces)
			{
				context.Services.AddScoped(@interface, type);
			}
		}

		context.Services.AddSingleton<IdentityCommonComponent>();
	}
}