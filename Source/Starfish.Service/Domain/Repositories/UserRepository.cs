using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public sealed class UserRepository : BaseRepository<DataContext, User, int>
{
	public UserRepository(IContextProvider provider)
		: base(provider)
	{
	}
}
