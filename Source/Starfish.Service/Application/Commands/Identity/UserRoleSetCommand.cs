using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class UserRoleSetCommand : Command<int, List<string>>
{
	public UserRoleSetCommand(int id, List<string> roles)
		: base(id, roles)
	{
	}
}