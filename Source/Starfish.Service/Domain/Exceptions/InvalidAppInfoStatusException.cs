namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用状态无效异常
/// </summary>
public class InvalidAppInfoStatusException : BadRequestException
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="message"></param>
	public InvalidAppInfoStatusException(string message)
		: base(message)
	{
	}
}