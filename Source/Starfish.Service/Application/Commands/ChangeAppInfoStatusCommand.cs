using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 修改应用信息状态命令
/// </summary>
public class ChangeAppInfoStatusCommand : Command<long, AppStatus>
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	/// <param name="status"></param>
	public ChangeAppInfoStatusCommand(long id, AppStatus status)
		: base(id, status)
	{
	}
}