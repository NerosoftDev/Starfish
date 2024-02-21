using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IUserQueryUseCase : IUseCase<GenericQueryInput<UserCriteria>, UserSearchOutput>;

internal record UserSearchOutput(List<UserItemDto> Result) : IUseCaseOutput;

internal class UserQueryUseCase : IUserQueryUseCase
{
	private readonly IUserRepository _repository;
	private readonly UserPrincipal _identity;

	public UserQueryUseCase(IUserRepository repository, UserPrincipal identity)
	{
		_repository = repository;
		_identity = identity;
	}

	public Task<UserSearchOutput> ExecuteAsync(GenericQueryInput<UserCriteria> input, CancellationToken cancellationToken = default)
	{
		if (!_identity.IsAuthenticated)
		{
			throw new AuthenticationException();
		}

		if (!_identity.IsInRole("SA"))
		{
			throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
		}

		var specification = input.Criteria.GetSpecification();
		var predicate = specification.Satisfy();

		return _repository.FindAsync(predicate, null, input.Skip, input.Count, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = task.Result.ProjectedAsCollection<UserItemDto>();
			                  return new UserSearchOutput(result);
		                  }, cancellationToken);
	}
}