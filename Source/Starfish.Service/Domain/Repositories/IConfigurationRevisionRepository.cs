using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public interface IConfigurationRevisionRepository : IBaseRepository<DataContext, ConfigurationRevision, long>
{
}