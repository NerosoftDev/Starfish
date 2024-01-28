using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

public class ConfigurationRevisionRepository : BaseRepository<DataContext, ConfigurationRevision, long>, IConfigurationRevisionRepository
{
	public ConfigurationRevisionRepository(IContextProvider provider)
		: base(provider)
	{
	}
}