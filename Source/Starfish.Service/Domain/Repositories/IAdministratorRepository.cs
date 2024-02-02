using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

internal interface IAdministratorRepository : IBaseRepository<DataContext, Administrator, long>
{
	Task<Administrator> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}