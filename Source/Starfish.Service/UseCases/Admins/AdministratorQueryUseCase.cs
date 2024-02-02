using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IAdministratorQueryUseCase : IUseCase<GenericQueryInput<AdministratorCriteria>, AdministratorQueryOutput>;

internal record AdministratorQueryOutput(List<AdministratorItemDto> Result);

internal class AdministratorQueryUseCase : IAdministratorQueryUseCase
{
	private readonly IAdministratorRepository _repository;
	private readonly UserPrincipal _user;

	public AdministratorQueryUseCase(IAdministratorRepository repository, UserPrincipal user)
	{
		_repository = repository;
		_user = user;
	}

	public Task<AdministratorQueryOutput> ExecuteAsync(GenericQueryInput<AdministratorCriteria> input, CancellationToken cancellationToken = default)
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

		return _repository.FindAsync(predicate, query => query.AsNoTracking().Include(t => t.User), input.Skip, input.Count, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = TypeAdapter.ProjectedAs<List<AdministratorItemDto>>(task.Result);
			                  return new AdministratorQueryOutput(result);
		                  }, cancellationToken);
	}
}