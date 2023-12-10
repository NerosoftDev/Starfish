using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 操作日志仓储
/// </summary>
public class OperateLogRepository : BaseRepository<DataContext, OperateLog, long>, IOperateLogRepository
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="provider"></param>
	public OperateLogRepository(IContextProvider provider)
		: base(provider)
	{
	}
}