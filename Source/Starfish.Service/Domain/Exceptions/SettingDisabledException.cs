namespace Nerosoft.Starfish.Domain;

public class SettingDisabledException : BadRequestException
{
	public SettingDisabledException(long appId, string environment)
		: base(string.Format(Resources.IDS_ERROR_SETTING_DISABLED, appId, environment))
	{
	}
}