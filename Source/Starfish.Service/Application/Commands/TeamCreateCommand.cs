using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 新建团队命令
/// </summary>
public sealed class TeamCreateCommand : Command
{
	/// <summary>
	/// 名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 描述
	/// </summary>
	public string Description { get; set; }
}
