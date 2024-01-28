using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 删除应用信息命令
/// </summary>
public class AppInfoDeleteCommand : Command<long>
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	public AppInfoDeleteCommand(long id)
		: base(id)
	{
	}
}