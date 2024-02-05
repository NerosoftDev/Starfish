namespace Nerosoft.Starfish.Domain;

public class ConfigurationDisabledException : BadRequestException
{
	public ConfigurationDisabledException(string id)
		: base(string.Format(Resources.IDS_ERROR_CONFIG_DISABLED, id))
	{
	}
}