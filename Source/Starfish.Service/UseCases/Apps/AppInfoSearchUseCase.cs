using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 应用信息搜索用例接口
/// </summary>
public interface IAppInfoSearchUseCase : IUseCase<AppInfoSearchInput, AppInfoSearchOutput>;

/// <summary>
/// 应用信息搜索用例输入
/// </summary>
/// <param name="Criteria"></param>
/// <param name="Page"></param>
/// <param name="Size"></param>
public record AppInfoSearchInput(AppInfoCriteria Criteria, int Page, int Size) : IUseCaseInput;

/// <summary>
/// 应用信息搜索用例输出
/// </summary>
/// <param name="Result"></param>
public record AppInfoSearchOutput(List<AppInfoItemDto> Result) : IUseCaseOutput;

/// <summary>
/// 应用信息搜索用例
/// </summary>
public class AppInfoSearchUseCase : IAppInfoSearchUseCase
{
	private readonly IAppInfoRepository _repository;
	private readonly UserPrincipal _user;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="user"></param>
	/// 
	public AppInfoSearchUseCase(IAppInfoRepository repository, UserPrincipal user)
	{
		_repository = repository;
		_user = user;
	}

	/// <inheritdoc />
	public Task<AppInfoSearchOutput> ExecuteAsync(AppInfoSearchInput input, CancellationToken cancellationToken = default)
	{
		if (input.Page <= 0)
		{
			throw new BadRequestException(Resources.IDS_ERROR_PAGE_NUMBER_MUST_GREATER_THAN_0);
		}

		if (input.Size <= 0)
		{
			throw new BadRequestException(Resources.IDS_ERROR_PAGE_SIZE_MUST_GREATER_THAN_0);
		}

		var predicate = input.Criteria.GetSpecification().Satisfy();
		return _repository.FindAsync(predicate, Permission, input.Page, input.Size, cancellationToken)
		                  .ContinueWith(task => new AppInfoSearchOutput(task.Result.ProjectedAsCollection<AppInfoItemDto>()), cancellationToken);

		IQueryable<AppInfo> Permission(IQueryable<AppInfo> query)
		{
			if (!_user.IsInRole("SA"))
			{
				var userId = _user.GetUserIdOfInt32();
				var teamQuery = _repository.SetOf<TeamMember>();
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