using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface ITeamQueryUseCase : IUseCase<TeamQueryInput, TeamQueryOutput>;

public record TeamQueryInput(TeamCriteria Criteria, int Skip, int Count) : IUseCaseInput;

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
		if (input.Skip < 0)
		{
			throw new BadRequestException(Resources.IDS_ERROR_PARAM_SKIP_CANNOT_BE_NEGATIVE);
		}

		if (input.Count <= 0)
		{
			throw new BadRequestException(Resources.IDS_ERROR_PARAM_COUNT_MUST_GREATER_THAN_0);
		}

		if (!_identity.IsAuthenticated)
		{
			throw new AuthenticationException();
		}
		
		var specification = input.Criteria.GetSpecification();

		var predicate = specification.Satisfy();

		return _repository.FindAsync(predicate, Permission, input.Skip, input.Count, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = task.Result.ProjectedAsCollection<TeamItemDto>();
			                  return new TeamQueryOutput(result);
		                  }, cancellationToken);

		IQueryable<Team> Permission(IQueryable<Team> query)
		{
			if (!_identity.IsInRole("SA"))
			{
				var memberQuery = _repository.Context.Set<TeamMember>();
				query = from team in query
				        join member in memberQuery on team.Id equals member.TeamId
				        where member.UserId == _identity.UserId
				        select team;
			}

			{
			}

			return query.OrderByDescending(t => t.Id);
		}
	}
}