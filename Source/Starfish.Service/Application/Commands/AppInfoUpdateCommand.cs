using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 更新应用信息命令
/// </summary>
public class AppInfoUpdateCommand : Command<long, AppInfoUpdateDto>
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	/// <param name="model"></param>
	public AppInfoUpdateCommand(long id, AppInfoUpdateDto model)
		: base(id, model)
	{
	}
}