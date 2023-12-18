using Nerosoft.Euonia.Application;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 
/// </summary>
public sealed class ApplicationServiceContext : ServiceContextBase
{
	/// <inheritdoc/>
	public override bool AutoRegisterApplicationService => true;
}
