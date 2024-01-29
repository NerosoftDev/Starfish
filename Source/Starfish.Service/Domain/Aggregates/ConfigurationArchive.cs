using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置归档
/// </summary>
public class ConfigurationArchive : Aggregate<long>
{
	private ConfigurationArchive()
	{ }

	/// <summary>
	/// 应用Id
	/// </summary>
	public string AppId { get; set; }

	/// <summary>
	/// 应用环境
	/// </summary>
	public string Environment { get; set; }

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

	internal static ConfigurationArchive Create(string appId, string environment)
	{
		var entity = new ConfigurationArchive()
		{
			AppId = appId,
			Environment = environment
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