using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 设置App密钥命令
/// </summary>
public class ConfigurationSetSecretCommand : Command
{
	public ConfigurationSetSecretCommand(string id, string secret)
	{
		Id = id;
		Secret = secret;
	}

	public string Id { get; set; }

	public string Secret { get; set; }
}