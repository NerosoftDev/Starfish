using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class UserRoleSetCommand : Command<string, List<string>>
{
	public UserRoleSetCommand(string id, List<string> roles)
		: base(id, roles)
	{
	}
}