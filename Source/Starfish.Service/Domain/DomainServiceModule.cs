using Nerosoft.Euonia.Modularity;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 领域模块上下文
/// </summary>
public sealed class DomainServiceModule : ModuleContextBase
{
	/// <inheritdoc/>
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		context.Services.AddBusinessObject(typeof(DomainServiceModule).Assembly);
	}
}