using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 应用详情查询用例接口
/// </summary>
public interface IAppInfoDetailUseCase : IUseCase<AppInfoDetailInput, AppInfoDetailOutput>;

/// <summary>
/// 应用详情查询用例输出
/// </summary>
/// <param name="Result"></param>
public record AppInfoDetailOutput(AppInfoDetailDto Result) : IUseCaseOutput;

/// <summary>
/// 应用详情查询用例输入
/// </summary>
/// <param name="Id"></param>
public record AppInfoDetailInput(string Id) : IUseCaseInput;

/// <summary>
/// 应用详情查询用例
/// </summary>
public class AppInfoDetailUseCase : IAppInfoDetailUseCase
{
	private readonly IAppInfoRepository _repository;
	private readonly UserPrincipal _user;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="user"></param>
	public AppInfoDetailUseCase(IAppInfoRepository repository, UserPrincipal user)
	{
		_repository = repository;
		_user = user;
	}

	/// <inheritdoc />
	public Task<AppInfoDetailOutput> ExecuteAsync(AppInfoDetailInput input, CancellationToken cancellationToken = default)
	{
		if (!_user.IsAuthenticated)
		{
			throw new AuthenticationException();
		}

		var predicate = AppInfoSpecification.IdEquals(input.Id).Satisfy();
		return _repository.GetAsync(predicate, Permission, cancellationToken)
		                  .ContinueWith(task => new AppInfoDetailOutput(task.Result.ProjectedAs<AppInfoDetailDto>()), cancellationToken);

		IQueryable<AppInfo> Permission(IQueryable<AppInfo> query)
		{
			if (!_user.IsInRole("SA"))
			{
				var teamQuery = _repository.Context.Set<TeamMember>();
				query = from app in query
				        join member in teamQuery on app.TeamId equals member.TeamId
				        where member.UserId == _user.UserId
				        select app;
			}

			{
			}

			return query;
		}
	}
}