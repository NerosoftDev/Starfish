using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public sealed class ConfigurationItemsUpdateCommand : Command
{
	public ConfigurationItemsUpdateCommand(string id, string mode, IDictionary<string, string> items)
	{
		Id = id;
		Mode = mode;
		Items = items;
	}

	public string Id { get; set; }

	/// <summary>
	/// 更新方式
	/// </summary>
	public string Mode { get; set; }

	/// <summary>
	/// 所有配置项
	/// </summary>
	public IDictionary<string, string> Items { get; set; }
}