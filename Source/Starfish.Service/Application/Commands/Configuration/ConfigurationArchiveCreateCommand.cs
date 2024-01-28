using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 创建配置归档命令
/// </summary>
public class ConfigurationArchiveCreateCommand : Command
{
	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

	/// <summary>
	/// 应用环境
	/// </summary>
	public string Environment { get; set; }
}