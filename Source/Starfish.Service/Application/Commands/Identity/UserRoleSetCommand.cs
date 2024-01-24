using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class UserRoleSetCommand : Command<long, List<string>>
{
	public UserRoleSetCommand(long id, List<string> roles)
		: base(id, roles)
	{
	}
}