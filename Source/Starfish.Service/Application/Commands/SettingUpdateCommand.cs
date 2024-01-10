using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class SettingUpdateCommand : Command
{
	public SettingUpdateCommand(long id)
	{
		Id = id;
	}

	public long Id { get; set; }

	public IDictionary<string, string> Data { get; set; }
}