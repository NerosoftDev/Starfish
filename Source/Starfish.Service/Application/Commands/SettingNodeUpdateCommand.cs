using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 更新配置节点命令
/// </summary>
public class SettingNodeUpdateCommand : Command
{
	public SettingNodeUpdateCommand(long id, string intent, string value)
	{
		Id = id;
		Intent = intent;
		Value = value;
	}

	/// <summary>
	/// 节点Id
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// 更新的属性
	/// </summary>
	public string Intent { get; set; }

	/// <summary>
	/// 节点值
	/// </summary>
	public string Value { get; set; }
}