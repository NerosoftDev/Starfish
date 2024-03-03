using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 获取符合条件的配置数量用例接口
/// </summary>
internal interface IGetConfigurationItemCountUseCase : IUseCase<GetConfigurationItemCountInput, GetConfigurationItemCountOutput>;

/// <summary>
/// 获取符合条件的配置数量用例输出
/// </summary>
/// <param name="Result"></param>
internal record GetConfigurationItemCountOutput(int Result) : IUseCaseOutput;

/// <summary>
/// 获取符合条件的配置数量用例输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Key"></param>
internal record GetConfigurationItemCountInput(string Id, string Key) : IUseCaseInput;

/// <summary>
/// 获取符合条件的配置数量用例
/// </summary>
internal class GetConfigurationItemCountUseCase : IGetConfigurationItemCountUseCase
{
	private readonly IConfigurationRepository _repository;
	private readonly UserPrincipal _identity;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="identity"></param>
	public GetConfigurationItemCountUseCase(IConfigurationRepository repository, UserPrincipal identity)
	{
		_repository = repository;
		_identity = identity;
	}

	/// <inheritdoc />
	public Task<GetConfigurationItemCountOutput> ExecuteAsync(GetConfigurationItemCountInput input, CancellationToken cancellationToken = default)
	{
		Func<IQueryable<ConfigurationItem>, IQueryable<ConfigurationItem>> action = null;
		if (!_identity.IsInRoles(["SA"]))
		{
			var configSet = _repository.Context.Set<Configuration>();
			var teamSet = _repository.Context.Set<Team>();
			var teamMemberSet = _repository.Context.Set<TeamMember>();
			var query = from c in configSet
			            join t in teamSet on c.TeamId equals t.Id
			            join tm in teamMemberSet on t.Id equals tm.TeamId
			            where tm.UserId == _identity.UserId
			            select c.Id;
			action = q => q.Where(t => query.Contains(t.ConfigurationId));
		}

		return _repository.GetItemCountAsync(input.Id, input.Key, action, cancellationToken)
		                  .ContinueWith(t => new GetConfigurationItemCountOutput(t.Result), cancellationToken);
	}
}