namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 应用信息查询条件
/// </summary>
public class AppInfoCriteria
{
	/// <summary>
	/// 团队Id
	/// </summary>
	public long TeamId { get; set; }

	/// <summary>
	/// 关键字
	/// </summary>
	/// <remarks>
	/// 匹配应用名称、应用描述、应用标识
	/// </remarks>
	public string Keyword { get; set; }

	/// <summary>
	/// 状态
	/// </summary>
	/// <remarks>
	/// <para>0-全部</para>
	/// <para>1-启用</para>
	/// <para>2-禁用</para>
	/// </remarks>
	public int Status { get; set; }
}