using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 创建配置版本命令
/// </summary>
public class SettingRevisionCreateCommand : Command
{
	/// <summary>
	/// 配置根节点Id
	/// </summary>
	public long RootId { get; set; }

	/// <summary>
	/// 说明
	/// </summary>
	public string Comment { get; set; }

	/// <summary>
	/// 版本号
	/// </summary>
	public string Version { get; set; }
}