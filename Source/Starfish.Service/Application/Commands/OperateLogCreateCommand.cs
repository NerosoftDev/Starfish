using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 创建日志命令
/// </summary>
public class OperateLogCreateCommand : Command
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
}