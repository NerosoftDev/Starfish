using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 创建应用信息命令
/// </summary>
public class AppInfoCreateCommand : Command<AppInfoCreateDto>
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="data"></param>
	public AppInfoCreateCommand(AppInfoCreateDto data)
		: base(data)
	{
	}
}