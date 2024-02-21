using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface ITeamMemberQueryUseCase : IUseCase<TeamMemberQueryInput, TeamMemberQueryOutput>;

internal record TeamMemberQueryOutput(List<TeamMemberDto> Result) : IUseCaseOutput;

internal record TeamMemberQueryInput(string Id) : IUseCaseInput;

internal class TeamMemberQueryUseCase : ITeamMemberQueryUseCase
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