using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 操作日志信息
/// </summary>
public sealed class OperateLog : Aggregate<long>
{
	private OperateLog()
	{
	}

	/// <summary>
	/// 模块
	/// </summary>
	public string Module { get; set; }

	/// <summary>
	/// 类型
	/// </summary>
	public string Type { get; set; }

	/// <summary>
	/// 描述
	/// </summary>
	public string Content { get; set; }

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

	internal static OperateLog Create(string module, string type, string content, string userName, DateTime operateTime, string error, string requestTraceId)
	{
		return new OperateLog
		{
			Module = module,
			Type = type,
			Content = content,
			UserName = userName,
			OperateTime = operateTime,
			Error = error,
			RequestTraceId = requestTraceId
		};
	}
}