using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置节点发布命令
/// </summary>
public class ConfigurationPublishCommand : Command
{
	public ConfigurationPublishCommand(long id, string version, string comment)
	{
		Id = id;
		Version = version;
		Comment = comment;
	}

	public long Id { get; set; }

	public string Version { get; set; }

	public string Comment { get; set; }
}