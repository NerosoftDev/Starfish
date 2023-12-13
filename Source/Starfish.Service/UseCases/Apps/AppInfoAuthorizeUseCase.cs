using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 应用信息认证用例接口
/// </summary>
public interface IAppInfoAuthorizeUseCase : IUseCase<AppInfoAuthorizeInput, AppInfoAuthorizeOutput>;

/// <summary>
/// 应用信息认证输入
/// </summary>
/// <param name="Code"></param>
/// <param name="Secret"></param>
public record AppInfoAuthorizeInput(string Code, string Secret) : IUseCaseInput;

/// <summary>
/// 应用信息认证输出
/// </summary>
/// <param name="Result"></param>
public record AppInfoAuthorizeOutput(bool Result) : IUseCaseOutput;

/// <summary>
/// 应用信息认证用例
/// </summary>
public class AppInfoAuthorizeUseCase : IAppInfoAuthorizeUseCase
{
	private readonly IAppInfoRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="repository"></param>
	public AppInfoAuthorizeUseCase(IAppInfoRepository repository)
	{
		_repository = repository;
	}

	/// <inheritdoc />
	public async Task<AppInfoAuthorizeOutput> ExecuteAsync(AppInfoAuthorizeInput input, CancellationToken cancellationToken = default)
	{
		var appInfo = await _repository.GetByCodeAsync(input.Code, cancellationToken);
		if (appInfo == null)
		{
			return new AppInfoAuthorizeOutput(false);
		}

		return new AppInfoAuthorizeOutput(appInfo.Secret == input.Secret);
	}
}