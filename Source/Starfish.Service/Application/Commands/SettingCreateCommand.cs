using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class SettingCreateCommand : Command
{
	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

	/// <summary>
	/// 环境名称
	/// </summary>
	public string Environment { get; set; }

	/// <summary>
	/// 描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 配置项内容
	/// </summary>
	public IDictionary<string, string> Data { get; set; }
}