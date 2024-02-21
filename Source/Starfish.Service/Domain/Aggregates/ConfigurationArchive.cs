using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置归档
/// </summary>
public sealed class ConfigurationArchive : Aggregate<string>
{
	private ConfigurationArchive()
	{
	}

	/// <summary>
	/// 配置数据
	/// </summary>
	public string Data { get; set; }

	/// <summary>
	/// 操作人
	/// </summary>
	public string Operator { get; set; }

	/// <summary>
	/// 归档时间
	/// </summary>
	public DateTime ArchiveTime { get; set; }

	public Configuration Configuration { get; set; }

	internal static ConfigurationArchive Create(string configId)
	{
		var entity = new ConfigurationArchive()
		{
			Id = configId
		};

		return entity;
	}

	internal void Update(string data, string @operator)
	{
		Data = data;
		Operator = @operator;
		ArchiveTime = DateTime.Now;

		RaiseEvent(new ConfigurationArchiveUpdatedEvent());
	}
}