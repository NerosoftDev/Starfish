using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 获取符合条件的配置列表用例接口
/// </summary>
public interface ISettingSearchUseCase : IUseCase<SettingSearchInput, SettingSearchOutput>;

/// <summary>
/// 获取符合条件的配置列表用例输出
/// </summary>
/// <param name="Result"></param>
public record SettingSearchOutput(List<SettingItemDto> Result) : IUseCaseOutput;

/// <summary>
/// 获取符合条件的配置列表用例输入
/// </summary>
/// <param name="Criteria"></param>
/// <param name="Page"></param>
/// <param name="Size"></param>
public record SettingSearchInput(SettingCriteria Criteria, int Page, int Size) : IUseCaseInput;

/// <summary>
/// 获取符合条件的配置列表用例
/// </summary>
public class SettingSearchUseCase : ISettingSearchUseCase
{
	private readonly ISettingRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public SettingSearchUseCase(ISettingRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task<SettingSearchOutput> ExecuteAsync(SettingSearchInput input, CancellationToken cancellationToken = default)
	{
		var specification = input.Criteria.GetSpecification();
		var predicate = specification.Satisfy();
		return _repository.Include(t => t.App)
		                  .FetchAsync(predicate, Collator, input.Page, input.Size, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = task.Result.ProjectedAsCollection<SettingItemDto>();
			                  return new SettingSearchOutput(result);
		                  }, cancellationToken);
	}

	private static IOrderedQueryable<Setting> Collator(IQueryable<Setting> query)
	{
		return query.OrderByDescending(t => t.AppId)
		            .ThenBy(t => t.Id);
	}
}