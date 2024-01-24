namespace Nerosoft.Starfish.Domain;

public class AppInfoNotEnabledException : BadRequestException
{
	public AppInfoNotEnabledException(long appId)
		: base(string.Format(Resources.IDS_ERROR_APPINFO_NOT_ENABLED, appId))
	{
	}
}