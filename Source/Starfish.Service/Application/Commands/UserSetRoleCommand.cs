using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class UserSetRoleCommand : Command<int, List<string>>
{
	public UserSetRoleCommand(int id, List<string> roles)
		: base(id, roles)
	{
	}
}