namespace Nerosoft.Starfish.Domain;

public class SettingDisabledException : BadRequestException
{
	public SettingDisabledException(long id)
		: base(string.Format(Resources.IDS_ERROR_SETTING_DISABLED, id))
	{
	}
}