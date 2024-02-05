namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置信息不存在异常
/// </summary>
public class ConfigurationNotFoundException : NotFoundException
{
	public ConfigurationNotFoundException(string id)
		: base(string.Format(Resources.IDS_ERROR_CONFIG_NOT_EXISTS, id))
	{
	}

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="teamId">团队Id</param>
	/// <param name="name">配置名称</param>
	public ConfigurationNotFoundException(string teamId, string name)
		: base(string.Format(Resources.IDS_ERROR_CONFIG_NOT_EXISTS, teamId, name))
	{
	}
}