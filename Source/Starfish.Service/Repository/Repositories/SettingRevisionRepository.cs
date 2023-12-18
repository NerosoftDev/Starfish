using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

public class SettingRevisionRepository : BaseRepository<DataContext, SettingRevision, long>, ISettingRevisionRepository
{
	public SettingRevisionRepository(IContextProvider provider)
		: base(provider)
	{
	}
}