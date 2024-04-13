namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 操作日志Dto
/// </summary>
public class OperateLogDto
{
	/// <summary>
	/// Id
	/// </summary>
	public long Id { get; set; }

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
	/// 错误消息
	/// </summary>
	public string Error { get; set; }

	/// <summary>
	/// 请求跟踪Id
	/// </summary>
	public string RequestTraceId { get; set; }
}