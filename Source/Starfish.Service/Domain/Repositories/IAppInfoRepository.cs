using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用信息仓储接口
/// </summary>
public interface IAppInfoRepository : IBaseRepository<DataContext, AppInfo, string>
{
	Task<int> CheckPermissionAsync(string appId, string userId, CancellationToken cancellationToken = default);
}