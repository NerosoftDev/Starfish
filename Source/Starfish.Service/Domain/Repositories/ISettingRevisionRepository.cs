using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public interface ISettingRevisionRepository : IBaseRepository<DataContext, SettingRevision, long>
{
}