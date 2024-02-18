namespace Nerosoft.Starfish.Transit;

/// <summary>
/// 同步配置到Redis请求Dto
/// </summary>
public class ConfigurationPushRedisRequestDto
{
	/// <summary>
	/// Redis连接字符串
	/// </summary>
	public string ConnectionString { get; set; }

	/// <summary>
	/// 数据库
	/// </summary>
	public int Database { get; set; }

	/// <summary>
	/// Redis键名称
	/// </summary>
	public string Key { get; set; }
}