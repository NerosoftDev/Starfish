using Nerosoft.Euonia.Domain;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置项
/// </summary>
public sealed class ConfigurationItem : Entity<long>
{
	private ConfigurationItem()
	{
	}

	public ConfigurationItem(string key)
	{
		Key = key;
	}

	internal ConfigurationItem(string key, string value)
		: this()
	{
		Key = key;
		Value = value;
	}

	public string ConfigurationId { get; set; }

	public string Key { get; set; }

	public string Value { get; set; }

	public Configuration Configuration { get; set; }
}