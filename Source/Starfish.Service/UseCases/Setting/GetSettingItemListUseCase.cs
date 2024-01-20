using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
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
/// <param name="Skip"></param>
/// <param name="Count"></param>
public record GetSettingItemListInput(long Id, string Environment, int Skip, int Count) : IUseCaseInput;

/// <summary>
/// 获取符合条件的配置列表用例
/// </summary>
public class GetSettingItemListUseCase : IGetSettingItemListUseCase
{
	private readonly ISettingRepository _repository;
	private readonly UserPrincipal _identity;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="identity"></param>
	public GetSettingItemListUseCase(ISettingRepository repository, UserPrincipal identity)
	{
		_repository = repository;
		_identity = identity;
	}

	/// <inheritdoc />
	public Task<GetSettingItemListOutput> ExecuteAsync(GetSettingItemListInput input, CancellationToken cancellationToken = default)
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

		return _repository.GetItemListAsync(input.Id, input.Environment, input.Skip, input.Count, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = task.Result.ProjectedAsCollection<SettingItemDto>();
			                  return new GetSettingItemListOutput(result);
		                  }, cancellationToken);
	}
}