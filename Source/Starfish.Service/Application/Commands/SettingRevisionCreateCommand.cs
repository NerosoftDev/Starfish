using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class SettingRevisionCreateCommand : Command
{
	public long SettingId { get; set; }
	public string Comment { get; set; }
	public string Version { get; set; }
}