using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class AdministratorAssignCommand : Command
{
	public string UserId { get; set; }

	public List<string> Roles { get; set; }
}