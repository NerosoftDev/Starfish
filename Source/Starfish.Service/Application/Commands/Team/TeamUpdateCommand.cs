using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

public class TeamUpdateCommand : Command
{
	public TeamUpdateCommand(long id, TeamEditDto data)
	{
		Id = id;
		Data = data;
	}

	public long Id { get; set; }

	public TeamEditDto Data { get; set; }
}