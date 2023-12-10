using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 操作日志仓储
/// </summary>
public interface IOperateLogRepository : IRepository<OperateLog, long>
{
}