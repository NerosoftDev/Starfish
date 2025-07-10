using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;
using Nerosoft.Starfish.UseCases;

namespace Nerosoft.Starfish.Application;

public class TeamApplicationService : BaseApplicationService, ITeamApplicationService
{
	public Task<List<TeamItemDto>> QueryAsync(TeamCriteria criteria, int skip, int count, CancellationToken cancellationToken = default)
	{
		var userCase = LazyServiceProvider.GetService<ITeamQueryUseCase>();
		var input = new GenericQueryInput<TeamCriteria>(criteria, skip, count);
		return userCase.ExecuteAsync(input, cancellationToken)
		               .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	public Task<int> CountAsync(TeamCriteria criteria, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ITeamCountUseCase>();
		var input = new TeamCountInput(criteria);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	public Task<TeamDetailDto> GetAsync(long id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ITeamDetailUseCase>();
		var input = new TeamDetailInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	public Task<long> CreateAsync(TeamEditDto data, CancellationToken cancellationToken = default)
	{
		var command = new TeamCreateCommand(data);
		return Bus.SendAsync<TeamCreateCommand, long>(command, cancellationToken);
	}

	public Task UpdateAsync(long id, TeamEditDto data, CancellationToken cancellationToken = default)
	{
		var command = new TeamUpdateCommand(id, data);
		return Bus.SendAsync(command, cancellationToken);
	}

	public Task<List<TeamMemberDto>> QueryMembersAsync(long id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ITeamMemberQueryUseCase>();
		var input = new TeamMemberQueryInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	public Task AppendMembersAsync(long id, List<long> userIds, CancellationToken cancellationToken = default)
	{
		var command = new TeamMemberEditCommand(id, userIds, "+");
		return Bus.SendAsync(command, cancellationToken);
	}

	public Task RemoveMembersAsync(long id, List<long> userIds, CancellationToken cancellationToken = default)
	{
		var command = new TeamMemberEditCommand(id, userIds, "-");
		return Bus.SendAsync(command, cancellationToken);
	}

	public Task QuitAsync(long id, CancellationToken cancellationToken = default)
	{
		var command = new TeamMemberEditCommand(id, [User.GetUserIdOfInt64()], "-");
		return Bus.SendAsync(command, cancellationToken);
	}
}