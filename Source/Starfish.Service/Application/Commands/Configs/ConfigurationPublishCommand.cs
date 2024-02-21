using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置节点发布命令
/// </summary>
public class ConfigurationPublishCommand : Command
{
	public ConfigurationPublishCommand(string id, string version, string comment)
	{
		Id = id;
		Version = version;
		Comment = comment;
	}

	public string Id { get; set; }

	public string Version { get; set; }

	public string Comment { get; set; }
}