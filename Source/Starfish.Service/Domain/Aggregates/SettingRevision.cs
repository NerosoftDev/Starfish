using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置修订版本
/// </summary>
public class SettingRevision : Entity<long>,
                               IHasCreateTime
{
	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

	/// <summary>
	/// 应用唯一编码
	/// </summary>
	public string AppCode { get; set; }

	/// <summary>
	/// 环境
	/// </summary>
	public string Environment { get; set; }

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
}