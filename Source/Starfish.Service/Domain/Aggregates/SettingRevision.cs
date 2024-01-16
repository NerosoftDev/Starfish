using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置修订版本
/// </summary>
public class SettingRevision : Entity<long>,
                               IHasCreateTime
{
	private SettingRevision()
	{
	}

	internal SettingRevision(string version, string comment, string data, string @operator)
		: this()
	{
		Version = version;
		Comment = comment;
		Data = data;
		Operator = @operator;
	}

	public long SettingId { get; set; }

	/// <summary>
	/// 配置数据
	/// </summary>
	public string Data { get; set; }

	/// <summary>
	/// 描述信息
	/// </summary>
	public string Comment { get; set; }

	/// <summary>
	/// 版本
	/// </summary>
	public string Version { get; set; }

	/// <summary>
	/// 操作人
	/// </summary>
	public string Operator { get; set; }

	/// <summary>
	/// 归档时间
	/// </summary>
	public DateTime CreateTime { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public Setting Setting { get; set; }
}