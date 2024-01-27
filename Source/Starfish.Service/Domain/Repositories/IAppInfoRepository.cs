﻿using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用信息仓储接口
/// </summary>
public interface IAppInfoRepository : IBaseRepository<DataContext, AppInfo, long>
{
	/// <summary>
	/// 通过Code获取应用信息
	/// </summary>
	/// <param name="code"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<AppInfo> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

	Task<int> CheckPermissionAsync(long appId, long userId, CancellationToken cancellationToken = default);
}