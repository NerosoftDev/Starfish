using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class AdministratorDeleteCommand : Command
{
	public AdministratorDeleteCommand(string userId)
	{
		UserId = userId;
	}

	public string UserId { get; set; }
}