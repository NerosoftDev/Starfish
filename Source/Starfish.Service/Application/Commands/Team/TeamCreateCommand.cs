using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 新建团队命令
/// </summary>
public sealed class TeamCreateCommand : Command
{
	public TeamCreateCommand(TeamEditDto data)
	{
		Data = data;
	}

	public TeamEditDto Data { get; set; }
}