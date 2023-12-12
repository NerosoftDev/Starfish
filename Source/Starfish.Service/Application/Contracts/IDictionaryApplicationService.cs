using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 字典应用服务
/// </summary>
public interface IDictionaryApplicationService : IApplicationService
{
	/// <summary>
	/// 获取可用的角色列表
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<DictionaryItemDto>> GetRoleItemsAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// 获取可用的应用环境列表
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<DictionaryItemDto>> GetEnvironmentItemsAsync(CancellationToken cancellationToken = default);
}