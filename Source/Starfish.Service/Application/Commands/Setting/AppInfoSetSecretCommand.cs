using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 设置App密钥命令
/// </summary>
public class AppInfoSetSecretCommand : Command
{
	public AppInfoSetSecretCommand(long id, string secret)
	{
		Id = id;
		Secret = secret;
	}

	public long Id { get; set; }

	public string Secret { get; set; }
}