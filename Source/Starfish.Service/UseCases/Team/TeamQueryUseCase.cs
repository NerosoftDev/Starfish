using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface ITeamQueryUseCase : IUseCase<TeamQueryInput, TeamQueryOutput>;

public record TeamQueryInput(TeamCriteria Criteria, int Page, int Size) : IUseCaseInput;

public record TeamQueryOutput(List<TeamItemDto> Result) : IUseCaseOutput;

public class TeamQueryUseCase : ITeamQueryUseCase
{
	private readonly ITeamRepository _repository;
	private readonly UserPrincipal _identity;

	public TeamQueryUseCase(ITeamRepository repository, UserPrincipal identity)
	{
		_repository = repository;
		_identity = identity;
	}

	public Task<TeamQueryOutput> ExecuteAsync(TeamQueryInput input, CancellationToken cancellationToken = default)
	{
		var specification = input.Criteria.GetSpecification();
		if (!_identity.IsInRole("SA"))
		{
			specification &= TeamSpecification.HasMember(_identity.GetUserIdOfInt32());
		}

		var predicate = specification.Satisfy();

		return _repository.FindAsync(predicate, query => query.Include(nameof(Team.Members)).OrderByDescending(t => t.Id), input.Page, input.Size, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = task.Result.ProjectedAsCollection<TeamItemDto>();
			                  return new TeamQueryOutput(result);
		                  }, cancellationToken);
	}
}