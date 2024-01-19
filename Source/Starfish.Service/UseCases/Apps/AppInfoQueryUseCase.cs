using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 应用信息搜索用例接口
/// </summary>
public interface IAppInfoQueryUseCase : IUseCase<AppInfoQueryInput, AppInfoQueryOutput>;

/// <summary>
/// 应用信息搜索用例输入
/// </summary>
/// <param name="Criteria"></param>
/// <param name="Skip"></param>
/// <param name="Count"></param>
public record AppInfoQueryInput(AppInfoCriteria Criteria, int Skip, int Count) : IUseCaseInput;

/// <summary>
/// 应用信息搜索用例输出
/// </summary>
/// <param name="Result"></param>
public record AppInfoQueryOutput(List<AppInfoItemDto> Result) : IUseCaseOutput;

/// <summary>
/// 应用信息搜索用例
/// </summary>
public class AppInfoQueryUseCase : IAppInfoQueryUseCase
{
	private readonly IAppInfoRepository _repository;
	private readonly UserPrincipal _identity;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="identity"></param>
	/// 
	public AppInfoQueryUseCase(IAppInfoRepository repository, UserPrincipal identity)
	{
		_repository = repository;
		_identity = identity;
	}

	/// <inheritdoc />
	public Task<AppInfoQueryOutput> ExecuteAsync(AppInfoQueryInput input, CancellationToken cancellationToken = default)
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

		var predicate = input.Criteria.GetSpecification().Satisfy();
		return _repository.FindAsync(predicate, Permission, input.Skip, input.Count, cancellationToken)
		                  .ContinueWith(task => new AppInfoQueryOutput(task.Result.ProjectedAsCollection<AppInfoItemDto>()), cancellationToken);

		IQueryable<AppInfo> Permission(IQueryable<AppInfo> query)
		{
			if (!_identity.IsInRole("SA"))
			{
				var userId = _identity.GetUserIdOfInt32();
				var teamQuery = _repository.Context.Set<TeamMember>();
				query = from app in query
				        join member in teamQuery on app.TeamId equals member.TeamId
				        where member.UserId == userId
				        select app;
			}

			{
			}

			return query.OrderByDescending(t => t.Id);
		}
	}
}