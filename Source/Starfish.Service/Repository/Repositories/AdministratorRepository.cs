using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

internal sealed class AdministratorRepository : BaseRepository<DataContext, Administrator, long>, IAdministratorRepository
{
	public AdministratorRepository(IContextProvider provider) 
		: base(provider)
	{
	}

	public Task<Administrator> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
	{
		return Context.Set<Administrator>().FirstOrDefaultAsync(t => t.UserId == userId, cancellationToken);
	}
}
