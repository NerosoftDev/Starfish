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
/// <param name="Items"></param>
public record AppInfoSearchOutput(List<AppInfoItemDto> Items) : IUseCaseOutput;

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
	public AppInfoSearchUseCase(IAppInfoRepository repository, UserPrincipal user)
	{
		_repository = repository;
		_user = user;
	}

	/// <inheritdoc />
	public Task<AppInfoSearchOutput> ExecuteAsync(AppInfoSearchInput input, CancellationToken cancellationToken = default)
	{
		var predicate = input.Criteria.GetSpecification().Satisfy();
		return _repository.FetchAsync(predicate, Collator, input.Page, input.Size, cancellationToken)
		                  .ContinueWith(task => new AppInfoSearchOutput(task.Result.ProjectedAsCollection<AppInfoItemDto>()), cancellationToken);

		IOrderedQueryable<AppInfo> Collator(IQueryable<AppInfo> query) => query.OrderByDescending(t => t.Id);
	}
}