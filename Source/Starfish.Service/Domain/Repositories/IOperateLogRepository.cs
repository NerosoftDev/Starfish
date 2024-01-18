using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 操作日志仓储
/// </summary>
public interface IOperateLogRepository : IBaseRepository<DataContext, OperateLog, long>
{
}