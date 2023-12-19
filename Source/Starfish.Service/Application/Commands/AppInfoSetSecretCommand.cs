using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 设置App密钥命令
/// </summary>
public class AppInfoSetSecretCommand : Command<long, string>
{
	public AppInfoSetSecretCommand(long id, string secret)
		: base(id, secret)
	{
	}
}