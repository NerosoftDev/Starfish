using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public sealed class ConfigurationItemsUpdateCommand : Command
{
	public ConfigurationItemsUpdateCommand(string id, IDictionary<string, string> items)
	{
		Id = id;
		Items = items;
	}

	public string Id { get; set; }

	/// <summary>
	/// 所有配置项
	/// </summary>
	public IDictionary<string, string> Items { get; set; }
}