namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用状态无效异常
/// </summary>
public class InvalidAppInfoStatusException : BadRequestException
{
	public InvalidAppInfoStatusException()
		: base(Resources.IDS_ERROR_APPINFO_STATUS_INVALID)
	{
	}

	public InvalidAppInfoStatusException(string message)
		: base(message)
	{
	}
}