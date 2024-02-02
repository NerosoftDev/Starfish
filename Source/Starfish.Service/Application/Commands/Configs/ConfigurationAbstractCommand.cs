using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public abstract class ConfigurationAbstractCommand : Command
{
	protected ConfigurationAbstractCommand(string appId, string environment)
	{
		AppId = appId;
		Environment = environment;
	}

	/// <summary>
	/// 应用Id
	/// </summary>
	public string AppId { get; set; }

	/// <summary>
	/// 应用环境
	/// </summary>
	public string Environment { get; set; }
}