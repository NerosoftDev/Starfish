using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 操作日志查询规约
/// </summary>
public class OperateLogSpecification
{
	/// <summary>
	/// 用户名等于
	/// </summary>
	/// <param name="userName"></param>
	/// <returns></returns>
	public static Specification<OperateLog> UserNameEquals(string userName)
	{
		return new DirectSpecification<OperateLog>(x => x.UserName == userName);
	}

	/// <summary>
	/// 用户名包含
	/// </summary>
	/// <param name="userName"></param>
	/// <returns></returns>
	public static Specification<OperateLog> UserNameContains(string userName)
	{
		return new DirectSpecification<OperateLog>(x => x.UserName.Contains(userName));
	}

	/// <summary>
	/// 类型等于
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static Specification<OperateLog> TypeEquals(string type)
	{
		return new DirectSpecification<OperateLog>(x => x.Type == type);
	}

	/// <summary>
	/// 操作时间在指定范围内
	/// </summary>
	/// <param name="minTime"></param>
	/// <param name="maxTime"></param>
	/// <returns></returns>
	public static Specification<OperateLog> TimeBetween(DateTime minTime, DateTime maxTime)
	{
		return new DirectSpecification<OperateLog>(x => x.OperateTime >= minTime && x.OperateTime <= maxTime);
	}

	/// <summary>
	/// 操作时间在指定时间之后
	/// </summary>
	/// <param name="minTime"></param>
	/// <returns></returns>
	public static Specification<OperateLog> TimeAfter(DateTime minTime)
	{
		return new DirectSpecification<OperateLog>(x => x.OperateTime >= minTime);
	}

	/// <summary>
	/// 操作时间在指定时间之前
	/// </summary>
	/// <param name="maxTime"></param>
	/// <returns></returns>
	public static Specification<OperateLog> TimeBefore(DateTime maxTime)
	{
		return new DirectSpecification<OperateLog>(x => x.OperateTime <= maxTime);
	}

	/// <summary>
	/// 请求跟踪Id等于
	/// </summary>
	/// <param name="requestTraceId"></param>
	/// <returns></returns>
	public static Specification<OperateLog> RequestTraceIdEquals(string requestTraceId)
	{
		return new DirectSpecification<OperateLog>(x => x.RequestTraceId == requestTraceId);
	}
}