using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 删除配置节点命令
/// </summary>
public class ConfigurationDeleteCommand : Command
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id">配置Id</param>
	public ConfigurationDeleteCommand(long id)
	{
		Id = id;
	}

	public long Id { get; set; }
}