using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 创建配置版本命令
/// </summary>
public class ConfigurationRevisionCreateCommand : Command
{
	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

	/// <summary>
	/// 说明
	/// </summary>
	public string Comment { get; set; }

	/// <summary>
	/// 版本号
	/// </summary>
	public string Version { get; set; }

	/// <summary>
	/// 应用环境
	/// </summary>
	public string Environment { get; set; }
}