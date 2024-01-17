using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置归档
/// </summary>
public class SettingArchive : Aggregate<long>
{
	private SettingArchive()
	{ }

	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

	/// <summary>
	/// 应用唯一编码
	/// </summary>
	public string AppCode { get; set; }

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

	internal static SettingArchive Create(long appId, string appCode, string environment)
	{
		var entity = new SettingArchive()
		{
			AppId = appId,
			AppCode = appCode,
			Environment = environment
		};

		return entity;
	}

	internal void Update(string data, string @operator)
	{
		Data = data;
		Operator = @operator;
		ArchiveTime = DateTime.Now;

		RaiseEvent(new SettingArchiveUpdatedEvent());
	}
}