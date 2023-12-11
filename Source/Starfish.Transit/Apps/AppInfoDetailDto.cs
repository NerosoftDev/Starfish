namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 应用详细信息Dto
/// </summary>
public class AppInfoDetailDto
{/// <summary>
 	/// 名称
 	/// </summary>
 	public string Name { get; set; }
 
 	/// <summary>
 	/// 唯一编码
 	/// </summary>
 	public string Code { get; set; }
 
 	/// <summary>
 	/// 密钥
 	/// </summary>
 	public string Secret { get; set; }
 
 	/// <summary>
 	/// 描述
 	/// </summary>
 	public string Description { get; set; }
 
 	/// <summary>
 	/// 状态
 	/// </summary>
 	public string Status { get; set; }
 
 	/// <summary>
 	/// 创建时间
 	/// </summary>
 	public DateTime CreateTime { get; set; }
 
 	/// <summary>
 	/// 更新时间
 	/// </summary>
 	public DateTime UpdateTime { get; set; }
	
}