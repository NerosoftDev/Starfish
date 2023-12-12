using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 应用信息仓储
/// </summary>
public class AppInfoRepository : BaseRepository<DataContext, AppInfo, long>, IAppInfoRepository
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="provider"></param>
	public AppInfoRepository(IContextProvider provider)
		: base(provider)
	{
	}

	/// <inheritdoc />
	public Task<AppInfo> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(code))
		{
			throw new BadRequestException(Resources.IDS_ERROR_APPINFO_CODE_REQUIRED);
		}

		code = code.Normalize(TextCaseType.Lower);
		var predicate = AppInfoSpecification.CodeEquals(code).Satisfy();
		return GetAsync(predicate, false, cancellationToken);
	}
}