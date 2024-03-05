namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 连接信息传输对象
/// </summary>
public class ConnectionInfoDto
{
	/// <summary>
	/// 配置Id
	/// </summary>
	public string ConfigurationId { get; set; }

	/// <summary>
	/// 配置名称
	/// </summary>
	public string ConfigurationName { get; set; }

	/// <summary>
	/// 连接Id
	/// </summary>
	public string ConnectionId { get; set; }

	/// <summary>
	/// 连接类型
	/// </summary>
	public string ConnectionType { get; set; }

	/// <summary>
	/// 连接时间
	/// </summary>
	public DateTime ConnectedTime { get; set; }
}