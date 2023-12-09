using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 操作日志信息
/// </summary>
public class Logs : Aggregate<long>
{
	/// <summary>
	/// 类型
	/// </summary>
	public string Type { get; set; }

	/// <summary>
	/// 描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 用户名
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	/// 操作时间
	/// </summary>
	public DateTime OperateTime { get; set; }

	/// <summary>
	/// 请求跟踪Id
	/// </summary>
	public string RequestTraceId { get; set; }
}