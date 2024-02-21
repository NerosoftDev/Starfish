namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置信息不存在异常
/// </summary>
public class ConfigurationNotFoundException : NotFoundException
{
	public ConfigurationNotFoundException()
		: base(Resources.IDS_ERROR_CONFIG_NOT_EXISTS)
	{
	}

	public ConfigurationNotFoundException(string id)
		: base(string.Format(Resources.IDS_ERROR_CONFIG_NOT_EXISTS_OF_ID, id))
	{
	}
}