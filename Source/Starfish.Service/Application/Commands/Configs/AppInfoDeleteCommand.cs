using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 删除应用信息命令
/// </summary>
public class AppInfoDeleteCommand : Command<string>
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	public AppInfoDeleteCommand(string id)
		: base(id)
	{
	}
}