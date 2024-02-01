using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 应用信息查询规约
/// </summary>
internal static class AppInfoSpecification
{
	/// <summary>
	/// Id等于
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static Specification<AppInfo> IdEquals(string id)
	{
		return new DirectSpecification<AppInfo>(t => t.Id == id);
	}

	/// <summary>
	/// 团队Id等于
	/// </summary>
	/// <param name="teamId"></param>
	/// <returns></returns>
	public static Specification<AppInfo> TeamIdEquals(string teamId)
	{
		return new DirectSpecification<AppInfo>(t => t.TeamId == teamId);
	}

	/// <summary>
	/// 应用名称等于
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Specification<AppInfo> NameEquals(string name)
	{
		name = name.Normalize(TextCaseType.Lower);
		return new DirectSpecification<AppInfo>(t => t.Name.ToLower() == name);
	}

	/// <summary>
	/// 应用名称包含
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Specification<AppInfo> NameContains(string name)
	{
		name = name.Normalize(TextCaseType.Lower);
		return new DirectSpecification<AppInfo>(t => t.Name.ToLower().Contains(name));
	}

	/// <summary>
	/// 应用描述包含
	/// </summary>
	/// <param name="description"></param>
	/// <returns></returns>
	public static Specification<AppInfo> DescriptionContains(string description)
	{
		description = description.Normalize(TextCaseType.Lower);
		return new DirectSpecification<AppInfo>(t => t.Description.Contains(description));
	}

	/// <summary>
	/// 状态等于
	/// </summary>
	/// <param name="status"></param>
	/// <returns></returns>
	public static Specification<AppInfo> StatusEquals(AppStatus status)
	{
		return new DirectSpecification<AppInfo>(t => t.Status == status);
	}

	/// <summary>
	/// 匹配关键字
	/// </summary>
	/// <param name="keyword"></param>
	/// <returns></returns>
	public static Specification<AppInfo> Matches(string keyword)
	{
		var nameSpecification = NameContains(keyword);
		var descSpecification = DescriptionContains(keyword);

		return nameSpecification | descSpecification;
	}
}