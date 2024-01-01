using Nerosoft.Euonia.Domain;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置信息
/// </summary>
public class SettingNode : Entity<long>
{
	private SettingNode()
	{
	}

	internal SettingNode(string key, string value)
		: this()
	{
		Key = key;
		Value = value;
	}

	public long SettingId { get; set; }

	public string Key { get; set; }

	public string Value { get; set; }

	public Setting Setting { get; set; }
}