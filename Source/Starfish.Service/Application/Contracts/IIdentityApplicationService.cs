using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 授权和认证应用服务接口
/// </summary>
public interface IIdentityApplicationService : IApplicationService
{
	/// <summary>
	/// 获取访问令牌
	/// </summary>
	/// <param name="type">方式：password-用户名密码；refresh_token-刷新已有Token；OTP-通过验证码获取Token</param>
	/// <param name="data"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<AuthResponseDto> GrantAsync(string type, Dictionary<string, string> data, CancellationToken cancellationToken = default);
}