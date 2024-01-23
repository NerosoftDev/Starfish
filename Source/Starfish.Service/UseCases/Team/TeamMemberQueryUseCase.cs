using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface ITeamMemberQueryUseCase : IUseCase<TeamMemberQueryInput, TeamMemberQueryOutput>;

public record TeamMemberQueryOutput(List<TeamMemberDto> Result) : IUseCaseOutput;

public record TeamMemberQueryInput(long Id) : IUseCaseInput;

public class TeamMemberQueryUseCase : ITeamMemberQueryUseCase
{
	private readonly ITeamRepository _repository;

	public TeamMemberQueryUseCase(ITeamRepository repository)
	{
		_repository = repository;
	}

	public Task<TeamMemberQueryOutput> ExecuteAsync(TeamMemberQueryInput input, CancellationToken cancellationToken = default)
	{
		return _repository.GetMembersAsync(input.Id, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var resul = task.Result.ProjectedAsCollection<TeamMemberDto>();
			                  return new TeamMemberQueryOutput(resul);
		                  }, cancellationToken);
	}
}