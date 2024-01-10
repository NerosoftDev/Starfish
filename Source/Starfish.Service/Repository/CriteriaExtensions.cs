using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 查询条件扩展方法
/// </summary>
public static class CriteriaExtensions
{
	/// <summary>
	/// 获取日志查询规约
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	public static Specification<OperateLog> GetSpecification(this OperateLogCriteria criteria)
	{
		Specification<OperateLog> specification = new TrueSpecification<OperateLog>();

		if (criteria == null)
		{
			return specification;
		}

		if (!string.IsNullOrWhiteSpace(criteria.UserName))
		{
			specification &= OperateLogSpecification.UserNameContains(criteria.UserName);
		}

		if (!string.IsNullOrWhiteSpace(criteria.Type))
		{
			specification &= OperateLogSpecification.TypeEquals(criteria.Type);
		}

		if (criteria.MinTime > DateTime.MinValue)
		{
			specification &= OperateLogSpecification.TimeAfter(criteria.MinTime.Value);
		}

		if (criteria.MaxTime > DateTime.MinValue)
		{
			specification &= OperateLogSpecification.TimeBefore(criteria.MaxTime.Value);
		}

		return specification;
	}

	/// <summary>
	/// 获取应用信息查询规约
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	public static Specification<AppInfo> GetSpecification(this AppInfoCriteria criteria)
	{
		Specification<AppInfo> specification = new TrueSpecification<AppInfo>();
		if (criteria == null)
		{
			return specification;
		}

		if (criteria.Status > 0)
		{
			var status = (AppStatus)criteria.Status;
			if (status != AppStatus.None && Enum.IsDefined(status))
			{
				specification &= AppInfoSpecification.StatusEquals(status);
			}
		}

		if (criteria.TeamId > 0)
		{
			specification &= AppInfoSpecification.TeamIdEquals(criteria.TeamId);
		}

		if (!string.IsNullOrWhiteSpace(criteria.Keyword))
		{
			specification &= AppInfoSpecification.NameContains(criteria.Keyword);
		}

		return specification;
	}

	/// <summary>
	/// 获取配置节点查询规约
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	public static Specification<Setting> GetSpecification(this SettingCriteria criteria)
	{
		Specification<Setting> specification = new TrueSpecification<Setting>();
		if (criteria == null)
		{
			return specification;
		}

		if (!string.IsNullOrWhiteSpace(criteria.AppCode))
		{
			specification &= SettingSpecification.AppCodeEquals(criteria.AppCode);
		}

		if (!string.IsNullOrWhiteSpace(criteria.Environment))
		{
			specification &= SettingSpecification.EnvironmentEquals(criteria.Environment);
		}

		return specification;
	}

	public static Specification<User> GetSpecification(this UserCriteria criteria)
	{
		Specification<User> specification = new TrueSpecification<User>();
		if (criteria == null)
		{
			return specification;
		}

		if (!string.IsNullOrWhiteSpace(criteria.Keyword))
		{
			specification &= UserSpecification.UserNameContains(criteria.Keyword);
		}

		if (!string.IsNullOrWhiteSpace(criteria.Role))
		{
			specification &= UserSpecification.HasRole(criteria.Role);
		}

		return specification;
	}

	public static Specification<Team> GetSpecification(this TeamCriteria criteria)
	{
		Specification<Team> specification = new TrueSpecification<Team>();
		if (criteria == null)
		{
			return specification;
		}

		if (!string.IsNullOrWhiteSpace(criteria.Keyword))
		{
			specification &= TeamSpecification.Matches(criteria.Keyword);
		}

		return specification;
	}
}