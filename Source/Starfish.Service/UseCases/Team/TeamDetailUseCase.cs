using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface ITeamDetailUseCase : IUseCase<TeamDetailInput, TeamDetailOutput>;

public record TeamDetailInput(long Id) : IUseCaseInput;

public record TeamDetailOutput(TeamDetailDto Result) : IUseCaseOutput;

public class TeamDetailUseCase : ITeamDetailUseCase
{
	private readonly ITeamRepository _repository;
	private readonly UserPrincipal _identity;

	public TeamDetailUseCase(ITeamRepository repository, UserPrincipal identity)
	{
		_repository = repository;
		_identity = identity;
	}

	public Task<TeamDetailOutput> ExecuteAsync(TeamDetailInput input, CancellationToken cancellationToken = default)
	{
		var specification = TeamSpecification.IdEquals(input.Id);
		if (!_identity.IsInRole("SA"))
		{
			specification &= TeamSpecification.HasMember(_identity.GetUserIdOfInt64());
		}

		var predicate = specification.Satisfy();
		return _repository.Include(t => t.Members)
		                  .GetAsync(predicate, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = TypeAdapter.ProjectedAs<TeamDetailDto>(task.Result);
			                  return new TeamDetailOutput(result);
		                  }, cancellationToken);
	}
}