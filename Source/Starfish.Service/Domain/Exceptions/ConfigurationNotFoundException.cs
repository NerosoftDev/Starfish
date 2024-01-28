namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置信息不存在异常
/// </summary>
public class ConfigurationNotFoundException : NotFoundException
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="appId">应用Id</param>
	/// <param name="environment">应用环境</param>
	public ConfigurationNotFoundException(long appId, string environment)
		: base(string.Format(Resources.IDS_ERROR_CONFIG_NOT_EXISTS, appId, environment))
	{
	}
}