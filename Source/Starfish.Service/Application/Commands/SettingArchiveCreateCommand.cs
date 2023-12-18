using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 创建配置归档命令
/// </summary>
public class SettingArchiveCreateCommand : Command
{
	/// <summary>
	/// 配置根节点Id
	/// </summary>
	public long RootId { get; set; }
}