using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

public interface IAdministratorApplicationService : IApplicationService
{
	Task<List<AdministratorItemDto>> QueryAsync(AdministratorCriteria criteria, int skip, int count, CancellationToken cancellationToken = default);

	Task<int> CountAsync(AdministratorCriteria criteria, CancellationToken cancellationToken = default);

	Task AssignAsync(AdministratorAssignDto data, CancellationToken cancellationToken = default);

	Task DeleteAsync(string userId, CancellationToken cancellationToken = default);
}