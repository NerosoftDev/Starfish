using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 获取符合条件的配置列表用例接口
/// </summary>
public interface IGetSettingItemListUseCase : IUseCase<GetSettingItemListInput, GetSettingItemListOutput>;

/// <summary>
/// 获取符合条件的配置列表用例输出
/// </summary>
/// <param name="Result"></param>
public record GetSettingItemListOutput(List<SettingItemDto> Result) : IUseCaseOutput;

/// <summary>
/// 获取符合条件的配置列表用例输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Environment"></param>
/// <param name="Page"></param>
/// <param name="Size"></param>
public record GetSettingItemListInput(long Id, string Environment, int Page, int Size) : IUseCaseInput;

/// <summary>
/// 获取符合条件的配置列表用例
/// </summary>
public class GetSettingItemListUseCase : IGetSettingItemListUseCase
{
	private readonly ISettingRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public GetSettingItemListUseCase(ISettingRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public Task<GetSettingItemListOutput> ExecuteAsync(GetSettingItemListInput input, CancellationToken cancellationToken = default)
	{
		return _repository.GetItemListAsync(input.Id, input.Environment, input.Page, input.Size, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = task.Result.ProjectedAsCollection<SettingItemDto>();
			                  return new GetSettingItemListOutput(result);
		                  }, cancellationToken);
	}
}