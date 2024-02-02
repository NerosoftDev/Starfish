using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IAdministratorCountUseCase : IUseCase<AdministratorCountInput, AdministratorCountOutput>;

internal record AdministratorCountInput(AdministratorCriteria Criteria);

internal record AdministratorCountOutput(int Result);

internal class AdministratorCountUseCase : IAdministratorCountUseCase
{
	private readonly IAdministratorRepository _repository;
	private readonly UserPrincipal _user;

	public AdministratorCountUseCase(IAdministratorRepository repository, UserPrincipal user)
	{
		_repository = repository;
		_user = user;
	}

	public Task<AdministratorCountOutput> ExecuteAsync(AdministratorCountInput input, CancellationToken cancellationToken = default)
	{
		if (_user.IsAuthenticated)
		{
			throw new AuthenticationException(Resources.IDS_MESSAGE_LOGS_AUTH_FAILED);
		}

		if (!_user.IsInRoles(["SA"]))
		{
			throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
		}

		var predicate = AdministratorSpecification.Matches(input.Criteria.Keyword).Satisfy();

		return _repository.CountAsync(predicate, query => query.AsNoTracking().Include(t => t.User), cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  return new AdministratorCountOutput(task.Result);
		                  }, cancellationToken);
	}
}