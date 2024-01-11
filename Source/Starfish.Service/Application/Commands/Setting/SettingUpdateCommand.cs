using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public class SettingUpdateCommand : SettingAbstractCommand
{
	public SettingUpdateCommand(long appId, string environment)
		: base(appId, environment)
	{
		AppId = appId;
		Environment = environment;
	}

	public IDictionary<string, string> Data { get; set; }
}