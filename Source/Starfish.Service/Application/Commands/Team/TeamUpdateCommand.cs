using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

public class TeamUpdateCommand : Command
{
	public TeamUpdateCommand(string id, TeamEditDto data)
	{
		Id = id;
		Data = data;
	}

	public string Id { get; set; }

	public TeamEditDto Data { get; set; }
}