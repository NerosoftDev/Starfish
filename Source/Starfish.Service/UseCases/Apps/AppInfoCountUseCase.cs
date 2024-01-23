using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 应用数量查询用例接口
/// </summary>
public interface IAppInfoCountUseCase : IUseCase<AppInfoCountInput, AppInfoCountOutput>;

/// <summary>
/// 应用数量查询用例输出
/// </summary>
/// <param name="Count"></param>
public record AppInfoCountOutput(int Count) : IUseCaseOutput;

/// <summary>
/// 应用数量查询用例输入
/// </summary>
/// <param name="Criteria"></param>
public record AppInfoCountInput(AppInfoCriteria Criteria) : IUseCaseInput;

/// <summary>
/// 应用数量查询用例
/// </summary>
public class AppInfoCountUseCase : IAppInfoCountUseCase
{
	private readonly IAppInfoRepository _repository;
	private readonly UserPrincipal _user;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="user"></param>
	public AppInfoCountUseCase(IAppInfoRepository repository, UserPrincipal user)
	{
		_repository = repository;
		_user = user;
	}

	/// <inheritdoc />
	public async Task<AppInfoCountOutput> ExecuteAsync(AppInfoCountInput input, CancellationToken cancellationToken = default)
	{
		var predicate = input.Criteria.GetSpecification().Satisfy();
		var count = await _repository.CountAsync(predicate, Permission, cancellationToken);
		return new AppInfoCountOutput(count);

		IQueryable<AppInfo> Permission(IQueryable<AppInfo> query)
		{
			if (!_user.IsInRole("SA"))
			{
				var userId = _user.GetUserIdOfInt64();
				var teamQuery = _repository.Context.Set<TeamMember>();
				query = from app in query
				        join member in teamQuery on app.TeamId equals member.TeamId
				        where member.UserId == userId
				        select app;
			}

			{
			}

			return query;
		}
	}
}