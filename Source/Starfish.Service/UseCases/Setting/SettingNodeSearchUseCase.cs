using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 获取符合条件的配置列表用例接口
/// </summary>
public interface ISettingNodeSearchUseCase : IUseCase<SettingNodeSearchInput, SettingNodeSearchOutput>;

/// <summary>
/// 获取符合条件的配置列表用例输出
/// </summary>
/// <param name="Result"></param>
public record SettingNodeSearchOutput(List<SettingNodeItemDto> Result) : IUseCaseOutput;

/// <summary>
/// 获取符合条件的配置列表用例输入
/// </summary>
/// <param name="Criteria"></param>
/// <param name="Page"></param>
/// <param name="Size"></param>
public record SettingNodeSearchInput(SettingNodeCriteria Criteria, int Page, int Size) : IUseCaseInput;

/// <summary>
/// 获取符合条件的配置列表用例
/// </summary>
public class SettingNodeSearchUseCase : ISettingNodeSearchUseCase
{
	private readonly ISettingNodeRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public SettingNodeSearchUseCase(ISettingNodeRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task<SettingNodeSearchOutput> ExecuteAsync(SettingNodeSearchInput input, CancellationToken cancellationToken = default)
	{
		var specification = input.Criteria.GetSpecification();
		var predicate = specification.Satisfy();
		return _repository.FetchAsync(predicate, Collator, input.Page, input.Size, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = task.Result.ProjectedAsCollection<SettingNodeItemDto>();
			                  return new SettingNodeSearchOutput(result);
		                  }, cancellationToken);
	}

	private static IOrderedQueryable<SettingNode> Collator(IQueryable<SettingNode> query)
	{
		return query.OrderByDescending(t => t.AppId)
		            .ThenBy(t => t.ParentId)
		            .ThenBy(t => t.Sort)
		            .ThenBy(t => t.Id);
	}
}