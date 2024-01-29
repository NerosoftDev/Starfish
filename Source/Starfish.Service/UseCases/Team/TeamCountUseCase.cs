using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface ITeamCountUseCase : IUseCase<TeamCountInput, TeamCountOutput>;

public record TeamCountInput(TeamCriteria Criteria) : IUseCaseInput;

public record TeamCountOutput(int Result) : IUseCaseOutput;

public class TeamCountUseCase : ITeamCountUseCase
{
	private readonly ITeamRepository _repository;
	private readonly UserPrincipal _identity;

	public TeamCountUseCase(ITeamRepository repository, UserPrincipal identity)
	{
		_repository = repository;
		_identity = identity;
	}

	public Task<TeamCountOutput> ExecuteAsync(TeamCountInput input, CancellationToken cancellationToken = default)
	{
		var specification = input.Criteria.GetSpecification();
		if (!_identity.IsInRole("SA"))
		{
			specification &= TeamSpecification.HasMember(_identity.UserId);
		}

		var predicate = specification.Satisfy();

		return _repository.Include(t => t.Members)
		                  .CountAsync(predicate, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  return new TeamCountOutput(task.Result);
		                  }, cancellationToken);
	}
}