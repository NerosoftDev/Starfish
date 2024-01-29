using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;
using Nerosoft.Starfish.UseCases;

namespace Nerosoft.Starfish.Application;

public class TeamApplicationService : BaseApplicationService, ITeamApplicationService
{
	public Task<List<TeamItemDto>> QueryAsync(TeamCriteria criteria, int skip, int count, CancellationToken cancellationToken = default)
	{
		var userCase = LazyServiceProvider.GetService<ITeamQueryUseCase>();
		var input = new TeamQueryInput(criteria, skip, count);
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

	public Task<TeamDetailDto> GetAsync(string id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ITeamDetailUseCase>();
		var input = new TeamDetailInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	public Task<string> CreateAsync(TeamEditDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ITeamCreateUseCase>();
		var input = new TeamCreateInput(data);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	public Task UpdateAsync(string id, TeamEditDto data, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ITeamUpdateUseCase>();
		var input = new TeamUpdateInput(id, data);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	public Task<List<TeamMemberDto>> QueryMembersAsync(string id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ITeamMemberQueryUseCase>();
		var input = new TeamMemberQueryInput(id);
		return useCase.ExecuteAsync(input, cancellationToken)
		              .ContinueWith(task => task.Result.Result, cancellationToken);
	}

	public Task AppendMembersAsync(string id, List<string> userIds, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ITeamMemberAppendUseCase>();
		var input = new TeamMemberAppendInput(id, userIds);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	public Task RemoveMembersAsync(string id, List<string> userIds, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ITeamMemberRemoveUseCase>();
		var input = new TeamMemberRemoveInput(id, userIds);
		return useCase.ExecuteAsync(input, cancellationToken);
	}

	public Task QuitAsync(string id, CancellationToken cancellationToken = default)
	{
		var useCase = LazyServiceProvider.GetService<ITeamMemberQuitUseCase>();
		var input = new TeamMemberQuitInput(id, User.UserId);
		return useCase.ExecuteAsync(input, cancellationToken);
	}
}