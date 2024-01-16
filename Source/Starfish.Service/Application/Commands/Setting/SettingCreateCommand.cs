namespace Nerosoft.Starfish.Application;

public class SettingCreateCommand : SettingAbstractCommand
{
	public SettingCreateCommand(long appId, string environment)
		: base(appId, environment)
	{
	}
	
	/// <summary>
	/// 描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 配置项内容
	/// </summary>
	public IDictionary<string, string> Data { get; set; }
}