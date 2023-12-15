using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class SettingArchiveCreateCommand : Command
{
	/// <summary>
	/// 配置根节点Id
	/// </summary>
	public long SettingId { get; set; }
}