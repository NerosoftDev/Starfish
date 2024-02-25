using System.Security.Authentication;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 获取符合条件的配置列表用例接口
/// </summary>
internal interface IGetConfigurationItemListUseCase : IUseCase<GetConfigurationItemListInput, GetConfigurationItemListOutput>;

/// <summary>
/// 获取符合条件的配置列表用例输出
/// </summary>
/// <param name="Result"></param>
internal record GetConfigurationItemListOutput(List<ConfigurationItemDto> Result) : IUseCaseOutput;

/// <summary>
/// 获取符合条件的配置列表用例输入
/// </summary>
/// <param name="Id"></param>
/// <param name="Key"></param>
/// <param name="Skip"></param>
/// <param name="Count"></param>
internal record GetConfigurationItemListInput(string Id, string Key, int Skip, int Count) : IUseCaseInput;

/// <summary>
/// 获取符合条件的配置列表用例
/// </summary>
internal class GetConfigurationItemListUseCase : IGetConfigurationItemListUseCase
{
	private readonly IConfigurationRepository _repository;
	private readonly UserPrincipal _identity;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="identity"></param>
	public GetConfigurationItemListUseCase(IConfigurationRepository repository, UserPrincipal identity)
	{
		_repository = repository;
		_identity = identity;
	}

	/// <inheritdoc />
	public Task<GetConfigurationItemListOutput> ExecuteAsync(GetConfigurationItemListInput input, CancellationToken cancellationToken = default)
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

		return _repository.GetItemListAsync(input.Id, input.Key, input.Skip, input.Count, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = task.Result.ProjectedAsCollection<ConfigurationItemDto>();
			                  return new GetConfigurationItemListOutput(result);
		                  }, cancellationToken);
	}
}