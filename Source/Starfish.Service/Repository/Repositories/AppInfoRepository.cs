using Microsoft.EntityFrameworkCore;
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
		return GetAsync(predicate, null, cancellationToken);
	}

	public async Task<int> CheckPermissionAsync(long appId, long userId, CancellationToken cancellationToken = default)
	{
		var query = from app in Context.Set<AppInfo>()
					join team in Context.Set<Team>() on app.TeamId equals team.Id
					join member in Context.Set<TeamMember>() on team.Id equals member.TeamId
					where app.Id == appId
					select new
					{
						member.UserId,
						IsOwner = member.UserId == team.OwnerId
					};

		var result = await query.ToListAsync(cancellationToken);

		var user = result.FirstOrDefault(x => x.UserId == userId);
		if (user == null)
		{
			return 0;
		}

		return user.IsOwner ? 1 : 2;
	}
}