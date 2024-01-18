using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 修改应用信息状态命令
/// </summary>
public class ChangeAppStatusCommand : Command
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	/// <param name="status"></param>
	public ChangeAppStatusCommand(long id, AppStatus status)
	{
		Id = id;
		Status = status;
	}

	public long Id { get; set; }

	public AppStatus Status { get; set; }
}