namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 操作日志查询条件
/// </summary>
public class OperateLogCriteria
{
	/// <summary>
	/// 模块
	/// </summary>
	public string Module { get; set; }

	/// <summary>
	/// 类型
	/// </summary>
	public string Type { get; set; }

	/// <summary>
	/// 用户名
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	/// 时间范围（最小）
	/// </summary>
	public DateTime? MinTime { get; set; }

	/// <summary>
	/// 时间范围（最大）
	/// </summary>
	public DateTime? MaxTime { get; set; }
}